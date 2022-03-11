using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("-------Movement Settings-------")]
    [SerializeField] private float _movementSpeed;
    [SerializeField, Range(0, 1)] private float _InstantMovementPercentage = 0.1f;

    [Header("-------Mesh Settings-------")]
    [SerializeField] private GameObject[] _meshStates;
    [SerializeField] private int[] _meshStateScores;
    [SerializeField] private float _meshGrowth = 0.1f;
    [SerializeField] private float _meshGrowthSpeed = 1f;
    [SerializeField] private int _scoreToStopGrowing = 15;

    [Header("-------Projectile Settings-------")]
    [SerializeField] private GameObject _lightningProjectilePrefab = null;
    [SerializeField] private Transform _projectileSocket = null;

    [Header("-------Other Settings-------")]
    [SerializeField] private Material _carpetMaterial = null;

    private PlayerInputActions _inputActions = null;
    private CharacterController _characterController = null;

    private Animator[] _animator;
    private PhotonView _view;

    private Vector3 _faceDirection = Vector3.zero;
    private int _score = 0;
    private int _playerId = 0;
    private int _currentMeshStateIndex = 0;


    private void Start()
    {
        _animator = GetComponentsInChildren<Animator>();
        _view = GetComponent<PhotonView>();
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Fire.performed += Interact;
        _characterController = GetComponent<CharacterController>();
        _inputActions.Player.Enable();
        _playerId = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        _faceDirection = transform.forward;

        _meshStates[0].SetActive(true);
        for (int i=1; i < _meshStates.Length; i++)
        {
            _meshStates[i].SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (_view.IsMine)
        {
            HandleMovement();
            UpdateMaterial();
        }

        _animator[_currentMeshStateIndex].SetBool("isRunning", _inputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero);
    }

    void HandleMovement()
    {
        Vector2 movementInput = _inputActions.Player.Move.ReadValue<Vector2>();
        if (movementInput != Vector2.zero)
            _faceDirection = new Vector3(movementInput.x, 0, movementInput.y);

        Vector3 currentVel = _characterController.velocity;
        Vector3 movement = Vector3.Lerp(new Vector3(currentVel.x, 0, currentVel.z), new Vector3(movementInput.x, 0, movementInput.y) * _movementSpeed, _InstantMovementPercentage);

        _characterController.SimpleMove(movement);
    }

    private void UpdateMaterial()
    {
        _carpetMaterial.SetVector("_PlayerPosition" + _playerId, _characterController.transform.position);
    }
    private void Interact(InputAction.CallbackContext context)
    {
        _projectileSocket.position = this.transform.position + _faceDirection * 2;
        PhotonNetwork.Instantiate(_lightningProjectilePrefab.name, _projectileSocket.position, Quaternion.LookRotation(_faceDirection));
    }

    public void DustPickedUp(int amount)
    {      
        _score += amount;
        StartCoroutine(Grow(amount));
    }

    public void TakeDustOff(int amount)
    {
        if (_score <= 0)
            return;

        _score = Mathf.Max(0, _score - amount);
        StartCoroutine(Shrink(amount));
    }

    IEnumerator Shrink(int amount)
    {        
        int score = _score + amount;
        for (int i = 0; i < amount; i++)
        {
            if (score < 0)
                yield return null;

            --score;
            if (_currentMeshStateIndex != 0 && _meshStateScores[_currentMeshStateIndex] > score)
            {
                _meshStates[_currentMeshStateIndex].SetActive(false);
                Vector3 scale = _meshStates[_currentMeshStateIndex].transform.localScale;
                --_currentMeshStateIndex;
                _meshStates[_currentMeshStateIndex].SetActive(true);
                _meshStates[_currentMeshStateIndex].transform.localScale = scale;
            }
            else
            {
                float elapsedTime = 0;
                Vector3 initialScale = _meshStates[_currentMeshStateIndex].transform.localScale;
                while (elapsedTime < _meshGrowthSpeed / amount)
                {
                    _meshStates[_currentMeshStateIndex].transform.localScale = Vector3.Lerp(initialScale * (1 - _meshGrowth), initialScale,
                        1 - elapsedTime / _meshGrowthSpeed * amount);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _meshStates[_currentMeshStateIndex].transform.localScale = initialScale * (1 - _meshGrowth);
            }
        }  

        yield return null;
    }

    //Increases scale when not needing to change sprite, changes sprite when score threshhold is reached
    IEnumerator Grow(int amount)
    {
        if (_score < _scoreToStopGrowing)
        {
            int score = _score - amount;
            for (int i = 0; i < amount; i++)
            {

                ++score;
                if (_currentMeshStateIndex != _meshStateScores.Length - 1 && _meshStateScores[_currentMeshStateIndex + 1] <= score)
                {
                    _meshStates[_currentMeshStateIndex].SetActive(false);
                    Vector3 scale = _meshStates[_currentMeshStateIndex].transform.localScale;
                    ++_currentMeshStateIndex;
                    _meshStates[_currentMeshStateIndex].SetActive(true);
                    _meshStates[_currentMeshStateIndex].transform.localScale = scale;
                }
                else
                {
                    float elapsedTime = 0;
                    Vector3 initialScale = _meshStates[_currentMeshStateIndex].transform.localScale;
                    while (elapsedTime < _meshGrowthSpeed / amount)
                    {
                        _meshStates[_currentMeshStateIndex].transform.localScale = Vector3.Lerp(initialScale, initialScale * (1 + _meshGrowth),
                            elapsedTime / _meshGrowthSpeed * amount);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    _meshStates[_currentMeshStateIndex].transform.localScale = initialScale * (1 + _meshGrowth);
                }
            }
        }    
        
        yield return null;
    }
}

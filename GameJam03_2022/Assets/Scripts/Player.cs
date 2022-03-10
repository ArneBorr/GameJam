using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField, Range(0, 1)] private float _InstantMovementPercentage = 0.1f;
    [SerializeField] private Material _carpetMaterial = null;
    [SerializeField] private int _playerId = 0;

    private PlayerInputActions _inputActions = null;
    private CharacterController _characterController = null;

    private Animator _animator;
    private PhotonView _view;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _view = GetComponent<PhotonView>();
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Fire.performed += Interact;
        _characterController = GetComponent<CharacterController>();
        _inputActions.Player.Enable();
    }

    void FixedUpdate()
    {
        if (_view.IsMine)
        {
            HandleMovement();
            UpdateMaterial();
        }
    }

    void HandleMovement()
    {
        Vector2 movementInput = _inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 currentVel = _characterController.velocity;
        Vector3 movement = Vector3.Lerp(new Vector3(currentVel.x, 0, currentVel.z), new Vector3(movementInput.x, 0, movementInput.y) * _movementSpeed, _InstantMovementPercentage);

        _characterController.SimpleMove(movement);
        _animator.SetBool("isRunning", movementInput != Vector2.zero);
    }

    private void UpdateMaterial()
    {
        _carpetMaterial.SetVector("_PlayerPosition" + _playerId, _characterController.transform.position);
    }
    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interact!");
    }
}

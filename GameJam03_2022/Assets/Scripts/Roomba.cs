using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Roomba : MonoBehaviour
{
    [Header("--- Gameobjects & Components ---")]
    [SerializeField] private GameObject _checkpointParent;
    [SerializeField] private GameObject _particlesParent;
    [SerializeField] private GameObject _particleWhoosh;
    [SerializeField] private GameObject _vacuumSucker;

    [Header("--- Gameplay ---")]
    [SerializeField] private float _drivingSpeed = 2f;
    [SerializeField] private AnimationCurve _movementSpeedCurve;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private AnimationCurve _rotationSpeedCurve;
    [SerializeField] private float _cooldown = 10f;
    [SerializeField] private float _initialCooldown = 5f;

    [SerializeField] private float _checkpointRadius = 15f;

    private List<ParticleSystem> _particles = new List<ParticleSystem>();
    private List<ParticleSystem> _particlesWhoosh = new List<ParticleSystem>();
    private List<Vector2> _checkpointPositions = new List<Vector2>();
    private Vector3 _target = Vector3.zero;
    private Quaternion _initialRotation = Quaternion.identity;
    private Vector3 _initialPosition = Vector3.zero;
    private AudioSource _audioSource = null;

    private float _deltaStationery = 0f;
    private float _deltaRotation = 0f;
    private float _deltaMovement = 0f;
    private float _whooshPlayAfter = 1f;
    private float _deltaParticles = 0f;
    private bool _isDriving = false;
    private bool _isRotating = false;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _audioSource = GetComponent<AudioSource>();

        foreach (Transform checkpoint in _checkpointParent.GetComponentsInChildren<Transform>())
        {
            // Retrieve all the checkpoint data (we don't need the y pos)
            _checkpointPositions.Add(new Vector2(checkpoint.position.x, checkpoint.position.z));
        }

        // Handle particle systems
        _particles.Add(_particlesParent.GetComponent<ParticleSystem>());
        foreach(ParticleSystem ps in _particlesParent.GetComponentsInChildren<ParticleSystem>()) 
        {
            _particles.Add(ps);
        }

        _particlesWhoosh.Add(_particleWhoosh.GetComponent<ParticleSystem>());
        foreach (ParticleSystem ps in _particleWhoosh.GetComponentsInChildren<ParticleSystem>())
        {
            _particlesWhoosh.Add(ps);
        }

        _deltaStationery = _initialCooldown;
        _vacuumSucker.SetActive(false);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // Decrease timer
        _deltaStationery -= Time.deltaTime;

        if (!_isDriving && !_isRotating)
        {
            if(_deltaStationery < 0f) Rotate();
        }
        else if (_isRotating) 
        {
            // Check if Roomba is facing target
            if (Vector3.Angle(_target - transform.position, transform.forward) < 0.5f) Drive();
            else 
            {
                // Rotate Roomba
                Quaternion rotation = Quaternion.LookRotation(_target - transform.position);
                _deltaRotation += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(_initialRotation, rotation, _rotationSpeedCurve.Evaluate(_deltaRotation * _rotationSpeed));
            }
        }
        else if(_isDriving)
        {
            // Check if Roomba has arrived
            if (Vector3.Distance(_target, transform.position) < 0.1f) Arrive();
            else 
            {
                // Make roomba travel
                _deltaMovement += Time.deltaTime;
                transform.position = Vector3.Lerp(_initialPosition, _target, _movementSpeedCurve.Evaluate(_deltaMovement * _drivingSpeed));

                _deltaParticles += Time.deltaTime;
                if(_deltaParticles > _whooshPlayAfter) 
                {
                    foreach (ParticleSystem ps in _particlesWhoosh) ps.Play();
                    _deltaParticles = 0f;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _checkpointRadius);
    }

    private void Rotate() 
    {
        _isRotating = true;
        _deltaRotation = 0f;
        _initialRotation = transform.rotation;

        // Get a random location on the map
        int randomId = 0;
        do randomId = Random.Range(0, _checkpointPositions.Count);
        while (Vector3.Distance(transform.position, new Vector3(
            _checkpointPositions[randomId].x,
            transform.position.y,
            _checkpointPositions[randomId].y)) < _checkpointRadius);

        _target = new Vector3(
            _checkpointPositions[randomId].x,
            transform.position.y,
            _checkpointPositions[randomId].y
            );

        Debug.Log("Roomba started rotating");
    }

    private void Drive() 
    {
        _isRotating = false;
        _isDriving = true;
        _deltaMovement = 0f;
        _initialPosition = transform.position;

        _vacuumSucker.SetActive(true);

        foreach (ParticleSystem ps in _particles) ps.Play();

        _audioSource.Play();

        Debug.Log("Roomba started driving");
    }

    private void Arrive() 
    {
        _deltaStationery = _cooldown;
        _isDriving = false;

        _vacuumSucker.SetActive(false);

        foreach (ParticleSystem ps in _particles) ps.Stop();

        StartCoroutine(FadeOut(_audioSource, 1));

        Debug.Log("Roomba arrived");
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}

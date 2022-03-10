using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 1;
    [SerializeField, Range(0, 1)]
    private float _InstantMovementPercentage = 0.1f;
    [SerializeField]
    private Material _carpetMaterial = null;
    [SerializeField]
    private int _playerId = 0;

    private PlayerInputActions _inputActions = null;
    private CharacterController _characterController = null;


    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Fire.performed += Interact;

        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
            Debug.LogError("PlayerController : OnEnable() : No Charactercontroller set");
        if (_carpetMaterial == null)
            Debug.LogError("PlayerController : OnEnable() : No CarpetMaterial set");
    }


    void FixedUpdate()
    {
        HandleMovement();
        UpdateMaterial();
    }

    void HandleMovement()
    {
        Vector2 movementInput = _inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 currentVel = _characterController.velocity;
        Vector3 movement = Vector3.Lerp(new Vector3(currentVel.x, 0, currentVel.z), new Vector3(movementInput.x, 0, movementInput.y) * _movementSpeed, _InstantMovementPercentage);

        _characterController.SimpleMove(movement);
    }

    private void UpdateMaterial()
    {
        _carpetMaterial.SetVector("_PlayerPosition", _characterController.transform.position);
        _carpetMaterial.GetVector("_PlayerPosition");
        Matrix4x4 m = _carpetMaterial.GetMatrix("_PlayerPositions");
        
        m.SetRow(_playerId, new Vector4(transform.position.x, transform.position.y, transform.position.z, 0));
        _carpetMaterial.SetMatrix("_PlayerPositions", m);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interact!");
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _gravity = -19.62f; // Gravedad (ej: -9.81 * 2)


    [Header("Look Settings")]
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _verticalLookLimit = 80.0f;

    [Header("Component References")]
    [SerializeField] private Transform _cameraTransform;

    private CharacterController _characterController;
    private PlayerInputActions _inputActions;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _xRotation = 0f;

    // Variables nuevas para la gravedad
    private float _velocityY;
    private bool _isGrounded; 

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_cameraTransform == null)
        {
            Debug.LogError("Error: La referencia a la Transform de la cámara no está asignada en el FirstPersonController.", this);
            this.enabled = false;
            return;
        }

        _inputActions = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        _inputActions.Player.Enable();

        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;
        _inputActions.Player.Look.performed += OnLookInput;
        _inputActions.Player.Look.canceled += OnLookInput;
        
        // Asumiendo que tienes una acción "Jump" configurada
        // _inputActions.Player.Jump.performed += OnJumpInput; 
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Look.performed -= OnLookInput;
        _inputActions.Player.Look.canceled -= OnLookInput;
        
        // _inputActions.Player.Jump.performed -= OnJumpInput; 

        _inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleGravity(); // ¡NUEVO! Maneja la caída
        HandleMovement();
        HandleLook();
    }

    // private void OnJumpInput(InputAction.CallbackContext context)
    // {
    //     if (_isGrounded)
    //     {
    //         _velocityY = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    //     }
    // }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void HandleGravity()
    {
        _isGrounded = _characterController.isGrounded;

        // Si estamos en el suelo, forzamos la velocidad de caída a un valor muy pequeño 
        // para que no se acumule la gravedad, pero siga "pegado" al suelo.
        if (_isGrounded && _velocityY < 0)
        {
            _velocityY = -2f; 
        }

        // Aplicar la gravedad continuamente
        _velocityY += _gravity * Time.deltaTime;
    }

    private void HandleMovement()
    {
        // Movimiento horizontal (eje X y Z)
        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x;
        
        // Movimiento vertical (eje Y)
        // Se añade la velocidad vertical (gravedad/salto) al vector de movimiento
        moveDirection.y = _velocityY; 
        
        // Mover al CharacterController
        _characterController.Move(moveDirection * Time.deltaTime * _movementSpeed);
    }

    private void HandleLook()
    {
        float mouseX = _lookInput.x * _mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = _lookInput.y * _mouseSensitivity * Time.deltaTime;
        
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -_verticalLookLimit, _verticalLookLimit);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}
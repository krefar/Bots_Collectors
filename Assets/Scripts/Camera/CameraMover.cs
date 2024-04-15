using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private PlayerInput _playerInput;

    private Vector2 _moveDirection;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Camera.Move.performed += OnMove;
        _playerInput.Camera.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        Move();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.action.ReadValue<Vector2>();
    }

    private void Move()
    {
        var scaledMoveSpeed = _moveSpeed * Time.deltaTime;
        var offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

        transform.position += offset;
    }
}

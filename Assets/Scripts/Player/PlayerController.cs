using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _chrCtr;

    [Space]

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Space]

    [Header("Mouse Settings")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _mouseSensitivity = 100f;

    private FootstepController _footsteps;
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }

    private float _xRotation = 0f;

    private Vector3 _velocity;


    private void Start()
    {
        _chrCtr = GetComponent<CharacterController>();
        _footsteps = GetComponent<FootstepController>();
    }

    private void Update()
    {
        Move();
        MouseRotation();
    }

    private void Move()
    {
        bool isGrounded = _chrCtr.isGrounded;
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isMoving = move.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

        _footsteps.IsMoving = isMoving;
        _footsteps.IsRunning = isRunning;

        float speed = isRunning ? _runSpeed : _walkSpeed;
        _chrCtr.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _chrCtr.Move(_velocity * Time.deltaTime);
    }


    private void MouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}

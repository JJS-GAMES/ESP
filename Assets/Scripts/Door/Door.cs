using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Collider _collider;
    [Space, SerializeField] private float _interactDistance = 5f;
    [SerializeField] private float _maxOpenAngle = 90f;
    [SerializeField] private float _openSensitivity = 3f;

    private bool _isInteracting;
    private Transform _playerCamera;
    private float _currentAngle;

    private Quaternion _closedRotation;

    private void Start()
    {
        _playerCamera = Camera.main.transform;
        _closedRotation = transform.rotation;
    }

    private void Update()
    {
        HandleInteraction();
        RotateDoor();
    }

    private void HandleInteraction()
    {
        Ray ray = new Ray(_playerCamera.position, _playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance))
        {
            Door door = hit.transform.GetComponentInParent<Door>();

            if (door == this)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _isInteracting = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }

                if (_isInteracting && Input.GetKey(KeyCode.E))
                {
                    Vector3 toPlayer = _playerCamera.position - transform.position;
                    float side = Vector3.Dot(transform.right, toPlayer);

                    float mouseX = Input.GetAxis("Mouse X");

                    if (side < 0)
                        mouseX = -mouseX;

                    _currentAngle += mouseX * _openSensitivity;
                    _currentAngle = Mathf.Clamp(_currentAngle, 0f, _maxOpenAngle);
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    _isInteracting = false;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }


    private void RotateDoor()
    {
        Quaternion targetRotation = _closedRotation * Quaternion.Euler(0, _currentAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}

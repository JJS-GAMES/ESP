using UnityEngine;
public class Door : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private float _openSpeed = 10f;

    private bool _isOpen;

    private Quaternion _closedRotation;
    private Quaternion _targetRotation;
    private void Start()
    {
        _closedRotation = transform.rotation;
        _targetRotation = _closedRotation;
    }
    public void Interaction()
    {

        _isOpen = !_isOpen;
        float angle = _isOpen ? _openAngle : 0f;

        _targetRotation = Quaternion.Euler(0, angle, 0) * _closedRotation;
    }
    private void Update()
    {
        if (transform.rotation != _targetRotation)
        {
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, _targetRotation, Time.deltaTime * _openSpeed);
            _collider.isTrigger = true;
        }
        else _collider.isTrigger = false;
    }
}

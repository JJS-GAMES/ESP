using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    [SerializeField, Range(1f, 20f)] private float _transitionSpeed = 10f;

    [Space, SerializeField] private Vector3 _fusePosition;
    [SerializeField] private Vector3 _fuseRotation;

    private Vector3 _defaultPosition;
    private Vector3 _defaultRotation;

    private bool _isFuse = false;

    private void Start()
    {
        _defaultPosition = transform.localPosition;
        _defaultRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        ChangingShootingMode();
    }

    private void ChangingShootingMode()
    {
        if (Input.GetKeyDown(KeyCode.V)) _isFuse = !_isFuse;

        Vector3 targetPosition = _isFuse ? _fusePosition : _defaultPosition;
        Quaternion targetRotation = Quaternion.Euler(_isFuse ? _fuseRotation : _defaultRotation);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, _transitionSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _transitionSpeed);
    }
}

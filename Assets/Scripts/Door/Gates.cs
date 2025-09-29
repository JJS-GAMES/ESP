using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField] private GameObject _body;

    [SerializeField] private float _openOffset = 10f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _player;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private Vector3 _targetPosition;

    private bool _isOpen;

    public bool IsOpen => _isOpen;
    public float Speed => _speed;

    private void Start()
    {
        _closedPosition = _body.transform.position;
        _openPosition = _closedPosition + new Vector3(0, _openOffset, 0);
        _targetPosition = _closedPosition;
    }

    private void Update()
    {
        _body.transform.position = Vector3.MoveTowards(
            _body.transform.position,
            _targetPosition,
            _speed * Time.deltaTime
        );
    }

    public void Open()
    {
        _targetPosition = _openPosition;

        _isOpen = true;
    }

    public void Close()
    {
        _targetPosition = _closedPosition;

        _isOpen = false;
    }
}

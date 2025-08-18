using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField] private GameObject _body;

    [SerializeField] private float _openOffset = 10f;
    [SerializeField] private float _openSpeed = 2f;
    [SerializeField] private float _detectDistance = 10f;
    [SerializeField] private Transform _player;


    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _closedPosition = _body.transform.position;
        _openPosition = _closedPosition + new Vector3(0, _openOffset, 0);
        _targetPosition = _closedPosition;
    }

    private void Update()
    {
        float dist = Vector3.Distance(_player.position, transform.position);

        if (dist <= _detectDistance)
            _targetPosition = _openPosition;
        else
            _targetPosition = _closedPosition;

        OpenGates();
    }

    private void OpenGates()
    {
        _body.transform.position = Vector3.MoveTowards(
            _body.transform.position,
            _targetPosition,
            _openSpeed * Time.deltaTime
        );
    }
}

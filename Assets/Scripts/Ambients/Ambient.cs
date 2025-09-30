using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Ambient : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _ambientClip;
    [Range(0f, 1f), SerializeField] private float _volume = 0.5f;
    [SerializeField] private float _maxDistance = 2f;
    [SerializeField] private float _minDistance = 1f;

    private AudioSource _ambientAudioSource;
    private BoxCollider _boxCollider;
    private Transform _player;
    private bool _playerInside = false;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;

        PlayerController player = FindFirstObjectByType<PlayerController>(); // Лучше реализовать в GameManager'e
        if (player != null)
        {
            _player = player.transform;
            _playerInside = _boxCollider.bounds.Contains(_player.position);
        }
        _ambientAudioSource = GetComponentInChildren<AudioSource>();
        if (_ambientAudioSource != null && _ambientClip != null)
        {
            _ambientAudioSource.clip = _ambientClip;
            _ambientAudioSource.volume = _volume;
            _ambientAudioSource.spatialBlend = 1f;
            _ambientAudioSource.dopplerLevel = 0;
            _ambientAudioSource.spread = 0;
            _ambientAudioSource.maxDistance = _maxDistance;
            _ambientAudioSource.minDistance = _minDistance;
            _ambientAudioSource.loop = true;
            _ambientAudioSource.Play();
        }
    }

    private void Update()
    {
        if (_player == null || _ambientAudioSource == null) return;

        Vector3 targetPosition;

        if (_playerInside)
        {
            targetPosition = _player.position;
        }
        else
        {
            targetPosition = ClampPositionToBounds(_player.position, _boxCollider.bounds);
        }

        _ambientAudioSource.transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player != null && other.transform == _player)
        {
            _playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null && other.transform == _player)
        {
            _playerInside = false;
        }
    }

    private Vector3 ClampPositionToBounds(Vector3 target, Bounds bounds)
    {
        float x = Mathf.Clamp(target.x, bounds.min.x, bounds.max.x);
        float y = Mathf.Clamp(target.y, bounds.min.y, bounds.max.y);
        float z = Mathf.Clamp(target.z, bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}

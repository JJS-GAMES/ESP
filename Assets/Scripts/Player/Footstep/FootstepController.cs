using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepClips;
    [SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

    [Header("Settings")]
    [SerializeField] private float _walkStepInterval = 0.6f;
    [SerializeField] private float _runStepInterval = 0.35f;

    private CharacterController _controller;
    private PlayerController _player;
    private float _stepTimer;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_controller.isGrounded && _player.IsWalking)
        {
            HandleFootsteps();
        }
        else
        {
            _stepTimer = 0f;
        }
    }

    private void HandleFootsteps()
    {
        float interval = _player.IsRunning ? _runStepInterval : _walkStepInterval;

        _stepTimer -= Time.deltaTime;
        if (_stepTimer <= 0f)
        {
            PlayFootstep();
            _stepTimer = interval;
        }
    }

    private void PlayFootstep()
    {
        if (_footstepClips.Length == 0) return;

        int index = Random.Range(0, _footstepClips.Length);

        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(_footstepClips[index], _volume);
    }
}
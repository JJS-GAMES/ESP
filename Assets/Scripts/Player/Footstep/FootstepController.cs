using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

    [Header("Surfaces")]
    [SerializeField] private FootstepMaterialSurface _defaultSurface;
    [SerializeField] private FootstepMaterialSurface[] _surfaces;

    [Header("Step Settings")]
    [SerializeField] private float _walkStepInterval = 0.6f;
    [SerializeField] private float _runStepInterval = 0.35f;

    private CharacterController _controller;
    private float _stepTimer;

    public bool IsMoving { get; set; }
    public bool IsRunning { get; set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller.isGrounded && IsMoving)
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
        float interval = IsRunning ? _runStepInterval : _walkStepInterval;

        _stepTimer -= Time.deltaTime;
        if (_stepTimer <= 0f)
        {
            PlayFootstep();
            _stepTimer = interval;
        }
    }

    private void PlayFootstep()
    {
        FootstepMaterialSurface surface = DetectSurface() ?? _defaultSurface;
        if (surface == null || surface.Clips.Length == 0) return;

        int index = Random.Range(0, surface.Clips.Length);
        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(surface.Clips[index], _volume);
    }

    private FootstepMaterialSurface DetectSurface()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            PhysicsMaterial mat = hit.collider.sharedMaterial;
            if (mat != null)
            {
                foreach (var surface in _surfaces)
                {
                    if (surface.Material == mat)
                    {
                        return surface;
                    }
                }
            }
        }

        return null;
    }
}
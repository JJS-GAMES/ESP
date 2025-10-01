using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Gates _gates;

    [SerializeField, Tooltip("ƒистанци€ до сбрасывани€ сканировани€(если бы отойдем от сканера, во врем€ сканировани€ сечатки)")] private float _cancelDistance = 3f;

    [Space, SerializeField] private float _interactionDuration = 5f;
    [SerializeField] private float _resetDuration = 5f;
    [SerializeField] private float _errorDuration = 2f;

    [Header("-------------------------")]

    [Header("UI")]
    [SerializeField] private Canvas _canvas;

    [Space, SerializeField] private GameObject _scannerUI;
    [SerializeField] private GameObject _errorUI;

    [Header("Welcome UI")]
    [Space, SerializeField] private TextMeshProUGUI _welcomeText;
    [SerializeField] private TextMeshProUGUI _requiredText;

    [Header("Scanning UI")]
    [Space, SerializeField] private Image _scanningImage;
    [SerializeField] private TextMeshProUGUI _scanningText;

    [Header("Completed UI")]
    [Space, SerializeField] private TextMeshProUGUI _completedText;
    [SerializeField] private TextMeshProUGUI _openingText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Waiting UI")]
    [Space, SerializeField] private TextMeshProUGUI _waitingText;

    [Header("Error UI")]
    [Space, SerializeField] private GameObject _errorPanel;

    [Header("-------------------------")]
    [Header("Audio")]
    [Space, SerializeField] private ScannerAudioController _scannerAudioController;
    [SerializeField] private float _volume = 0.8f;

    [Header("Sounds")]
    [Space, SerializeField] private AudioClip _completedMessageClip;
    [SerializeField] private AudioClip _errorMessageClip;

    [SerializeField] private AudioClip _scanningMessageClip;

    private Coroutine _interactionCoroutine;
    private Transform _player;
    private bool _isScanning = false;
    private bool _isInteraction = false;

    private void Start()
    {
        if (_scannerAudioController == null) _scannerAudioController = GetComponentInChildren<ScannerAudioController>();
        else _scannerAudioController.Init(_volume);
        SetUIDefault();
    }
    private void Update()
    {
        if (_isScanning && _player != null)
        {
            float dist = Vector3.Distance(transform.position, _player.position);
            if (dist > _cancelDistance)
            {
                StopScanning();
            }
        }
    }

    public void BeginScanning(Transform player)
    {
        if (_interactionCoroutine == null && !_isInteraction && !_isScanning)
        {
            _player = player;
            _interactionCoroutine = StartCoroutine(Interaction());
        }
    }

    private IEnumerator Interaction()
    {
        if (_gates.IsOpen || _isScanning || _isInteraction)
        {
            _scannerUI?.gameObject.SetActive(false);

            _errorPanel?.gameObject.SetActive(true);
            _errorUI?.gameObject.SetActive(true);

            if (_errorMessageClip != null)
            {
                _scannerAudioController?.PlaySoundOneShot(_errorMessageClip);
            }

            yield return new WaitForSeconds(_errorDuration);

            _scannerUI?.gameObject.SetActive(true);
            _errorUI?.gameObject.SetActive(false);
            _errorPanel?.gameObject.SetActive(false);

            if (!_isScanning && !_isInteraction)
            {
                SetUIDefault() ;
                StopScanning() ;
                yield break;
            }
        }
        else
        {
            _isInteraction = true;
            _isScanning = true;

            _welcomeText?.gameObject.SetActive(false);
            _requiredText?.gameObject.SetActive(false);

            _scanningImage?.gameObject.SetActive(true);
            _scanningText?.gameObject.SetActive(true);

            if (_scanningMessageClip != null)
            {
                _scannerAudioController?.PlayLoopSound(_scanningMessageClip);
            }

            float elapsed = 0f;
            while (elapsed < _interactionDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _interactionDuration;
                _scanningImage.fillAmount = Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }

            _scanningImage.fillAmount = 1f;
            _gates.Open();

            _scanningImage?.gameObject.SetActive(false);
            _scanningText?.gameObject.SetActive(false);

            _scannerAudioController?.StopLoopSound();

            _isScanning = false;

            if (_completedMessageClip != null)
            {
                _scannerAudioController?.PlaySoundOneShot(_completedMessageClip);
            }

            _completedText?.gameObject.SetActive(true);
            _openingText?.gameObject.SetActive(true);

            yield return new WaitForSeconds(_gates.Speed);

            _completedText?.gameObject.SetActive(true);
            _openingText?.gameObject.SetActive(false);
            _timerText?.gameObject.SetActive(true);

            float elapsed2 = 0f;

            while (elapsed2 < _resetDuration)
            {
                elapsed2 += Time.deltaTime;
                float remaining = _resetDuration - elapsed2;
                _timerText.text = $"Closes in {Mathf.RoundToInt(remaining)} s";
                yield return null;
            }

            _completedText?.gameObject.SetActive(false);
            _timerText?.gameObject.SetActive(false);

            _waitingText?.gameObject.SetActive(true);

            _scanningImage.fillAmount = 0f;
            _gates.Close();

            yield return new WaitForSeconds(_gates.Speed);

            _waitingText?.gameObject.SetActive(false);
            _completedText?.gameObject.SetActive(true);

            if (_completedMessageClip != null)
            {
                _scannerAudioController?.PlaySoundOneShot(_completedMessageClip);
            }

            yield return new WaitForSeconds(2f);

            _completedText?.gameObject.SetActive(false);
            _welcomeText?.gameObject.SetActive(true);
            _requiredText?.gameObject.SetActive(true);

            _isInteraction = false;

            _interactionCoroutine = null;
        }
    }

    public void StopScanning()
    {
        if (_interactionCoroutine != null)
        {
            StopCoroutine(_interactionCoroutine);
            _scannerAudioController?.StopLoopSound();
            _interactionCoroutine = null;
            _isScanning = false;
            _isInteraction = false;

            SetUIDefault();
        }
    }

    private void SetUIDefault()
    {
        _scanningImage.fillAmount = 0f;

        _scannerUI?.gameObject.SetActive(true);

        _welcomeText?.transform.parent?.gameObject.SetActive(true);
        _welcomeText?.gameObject.SetActive(true);
        _requiredText?.gameObject.SetActive(true);

        _scanningImage?.transform.parent?.gameObject.SetActive(true);
        _scanningImage?.gameObject.SetActive(false);
        _scanningText?.gameObject.SetActive(false);

        _waitingText?.transform.parent?.gameObject.SetActive(true);
        _waitingText?.gameObject.SetActive(false);

        _completedText?.transform.parent?.gameObject.SetActive(true);
        _completedText?.gameObject.SetActive(false);
        _openingText?.gameObject.SetActive(false);
        _timerText?.gameObject.SetActive(false);

        _errorUI?.gameObject.SetActive(false);

        _errorPanel?.transform.parent?.gameObject.SetActive(true);
        _errorPanel?.gameObject.SetActive(false);
    }
}

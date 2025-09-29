using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetinaScanner : MonoBehaviour
{
    [SerializeField] private Gates _gates;
    [SerializeField] private float _interactionDuration = 5f;
    [SerializeField] private float _resetDuration = 5f;
    [SerializeField] private float _errorDuration = 2f;

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

    private bool _isScanning = false;

    private void Start()
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
    public IEnumerator Interaction()
    {
        if (_gates.IsOpen || _isScanning)
        {
            _scannerUI?.gameObject.SetActive(false);

            _errorPanel?.gameObject.SetActive(true);
            _errorUI?.gameObject.SetActive(true);

            yield return new WaitForSeconds(_errorDuration);

            _scannerUI?.gameObject.SetActive(true);

            _errorUI?.gameObject.SetActive(false);
            _errorPanel?.gameObject.SetActive(false);
        }
        else
        {
            _isScanning = true;

            float elapsed = 0f;

            _welcomeText?.gameObject.SetActive(false);
            _requiredText?.gameObject.SetActive(false);

            _scanningImage?.gameObject.SetActive(true);
            _scanningText?.gameObject.SetActive(true);

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

            yield return new WaitForSeconds(2f);

            _completedText?.gameObject.SetActive(false);
            _welcomeText?.gameObject.SetActive(true);
            _requiredText?.gameObject.SetActive(true);

            _isScanning = false;
        }
    }
}

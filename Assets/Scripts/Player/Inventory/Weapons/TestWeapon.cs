using DG.Tweening;
using System;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    [SerializeField, Range(1f, 20f)] private float _transitionSpeed = 10f;

    [Space, SerializeField] private Vector3 _defaultPosition;
    [SerializeField] private Vector3 _defaultRotation;

    [Space, SerializeField] private Vector3 _fusePosition;
    [SerializeField] private Vector3 _fuseRotation;

    [Space, SerializeField] private Vector3 _onUnequipPosition;
    [SerializeField] private Vector3 _onUnequipRotation;

    private bool _isEquiping = false;
    private bool _isActive = false;
    private bool _isFuse = false;

    private void Start()
    {
        transform.localPosition = _onUnequipPosition;
        transform.localRotation = Quaternion.Euler(_onUnequipRotation);
    }
    private void Update()
    {
        if (!gameObject.activeSelf) return;
        ChangingShootingMode();
    }

    private void ChangingShootingMode()
    {
        if (Input.GetKeyDown(KeyCode.V) && !_isEquiping && _isActive)
        {
            _isFuse = !_isFuse;
            Vector3 targetPos = _isFuse ? _fusePosition : _defaultPosition;
            Vector3 targetRot = _isFuse ? _fuseRotation : _defaultRotation;

            transform.DOLocalMove(targetPos, 0.15f).SetEase(Ease.OutSine);
            transform.DOLocalRotate(targetRot, 0.15f).SetEase(Ease.OutSine);
        }
    }

    public void OnEquip(Action onComplete = null)
    {
        if (_isEquiping) return;
        _isEquiping = true;

        transform.DOKill();

        transform.DOLocalMove(_defaultPosition, 0.25f).SetEase(Ease.OutBack);
        transform.DOLocalRotate(_defaultRotation, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _isEquiping = false;
                _isActive = true;
                onComplete?.Invoke();
            });
    }

    public void OnUnequip(Action onComplete = null)
    {
        if (_isEquiping) return;
        _isEquiping = true;

        transform.DOKill();

        transform.DOLocalMove(_onUnequipPosition, 0.2f).SetEase(Ease.InSine);
        transform.DOLocalRotate(_onUnequipRotation, 0.2f).SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                _isEquiping = false;
                _isActive = false;
                onComplete?.Invoke();
            });
    }

}

using System;
using UnityEngine;
using DG.Tweening;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] protected Vector3 _equipPosition;
    [SerializeField] protected Vector3 _equipRotation;

    [Space, SerializeField] protected Vector3 _onUnequipPosition;
    [SerializeField] protected Vector3 _onUnequipRotation;

    [Space, SerializeField] protected Vector3 _fusePosition;
    [SerializeField] protected Vector3 _fuseRotation;

    protected bool _isActive;
    protected bool _isEquiping;
    protected bool _isFuse;

    public bool IsActive => _isActive;
    public bool IsEquiping => _isEquiping;
    private void Start()
    {
        transform.localPosition = _onUnequipPosition;
        transform.localRotation = Quaternion.Euler(_onUnequipRotation);
    }
    public virtual void OnEquip(Action onComplete = null)
    {
        _isEquiping = true;
        transform.DOKill();

        transform.DOLocalMove(_equipPosition, 0.25f).SetEase(Ease.OutBack);
        transform.DOLocalRotate(_equipRotation, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _isEquiping = false;
                _isActive = true;
                onComplete?.Invoke();
            });
    }

    public virtual void OnUnequip(Action onComplete = null)
    {
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

    public virtual void ToggleFuseMode()
    {
        if (!_isActive || _isEquiping) return;

        _isFuse = !_isFuse;

        Vector3 targetPos = _isFuse ? _fusePosition : _equipPosition;
        Vector3 targetRot = _isFuse ? _fuseRotation : _equipRotation;

        transform.DOLocalMove(targetPos, 0.15f).SetEase(Ease.OutSine);
        transform.DOLocalRotate(targetRot, 0.15f).SetEase(Ease.OutSine);
    }

    public abstract void Fire();
    public abstract void Reload();
}

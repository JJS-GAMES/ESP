using System;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private BaseWeapon _weapon;

    public BaseWeapon Weapon => _weapon;

    private void Start()
    {
        _weapon = GetComponentInChildren<BaseWeapon>();
    }
    public void OnEquip(Action onComplete = null)
    {
        _weapon?.OnEquip(onComplete);
    }

    public void OnUnequip(Action onComplete = null)
    {
        _weapon?.OnUnequip(onComplete);
    }
}

using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TestWeapon[] _weapons;
    [SerializeField] private float _switchCooldown = 0.3f;

    private int _currentWeaponIndex = -1;

    private bool _isSwitching = false;
    private void Start()
    {
        if (_weapons == null || _weapons.Length == 0) return;

        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if(_isSwitching) return;

        int previousWeapon = _currentWeaponIndex;

        if (Input.GetKeyDown(KeyCode.Alpha1)) _currentWeaponIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _currentWeaponIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) _currentWeaponIndex = 2;

        if (_currentWeaponIndex != previousWeapon && _currentWeaponIndex >= 0 && _currentWeaponIndex < _weapons.Length)
            StartCoroutine(EquipWeapon(_currentWeaponIndex));
    }

    private IEnumerator EquipWeapon(int index)
    {
        _isSwitching = true;

        for (int i = 0; i < _weapons.Length; i++)
        {
            if (i == index) continue;

            bool finished = false;
            _weapons[i].OnUnequip(() => finished = true);
            while (!finished) yield return null;
        }

        _weapons[index].OnEquip(() => _isSwitching = false);
    }


}

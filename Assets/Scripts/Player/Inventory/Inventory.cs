using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TestWeapon[] _weapons;

    private int _currentWeaponIndex = -1;
    private void Start()
    {
        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentWeaponIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentWeaponIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentWeaponIndex = 2;
        }

        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(i == _currentWeaponIndex);
        }
    }

}

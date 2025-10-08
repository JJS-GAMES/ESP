using UnityEngine;

public class TestFirstWeapon : BaseWeapon
{
    [Space, SerializeField] private int _ammo = 30;
    [SerializeField] private int _maxAmmo = 30;

    private void Update()
    {
        if (!_isActive) return;

        if (Input.GetMouseButtonDown(0)) Fire();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        if (Input.GetKeyDown(KeyCode.V)) ToggleFuseMode();
    }

    public override void Fire()
    {
        if (_isFuse) return;
        if (_ammo <= 0)
        {
            Debug.Log("Патроны закончились!");
            return;
        }

        _ammo--;
        Debug.Log($"Пиу! Патрон осталось: {_ammo}");
    }

    public override void Reload()
    {
        _ammo = _maxAmmo;
        Debug.Log("Перезарядка!");
    }
}
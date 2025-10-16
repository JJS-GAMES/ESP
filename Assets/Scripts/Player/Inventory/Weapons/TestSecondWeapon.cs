using UnityEngine;

public class TestSecondWeapon : BaseWeapon
{
    [SerializeField] private Light _flashlight;
    private bool _isOn;

    private void Update()
    {
        if (!_isActive)
        {
            _flashlight.gameObject.SetActive(false);
            return;
        }

        if(Input.GetMouseButtonDown(0)) Fire();
        if(Input.GetKeyDown(KeyCode.R)) Reload();
        if(Input.GetKeyDown(KeyCode.V)) ToggleFuseMode();
    }
    public override void Fire()
    {
        _isOn = !_isOn;

        _flashlight.gameObject.SetActive(_isOn);
    }

    public override void Reload()
    {
        // Flashlight doesn't use ammo, so reloading is unnecessary.
    }
}

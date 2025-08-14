using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _cursorVisible = false;
    private void Start()
    {
        ToggleCursor(_cursorVisible);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.visible = toggle;
        if (toggle) Cursor.lockState = CursorLockMode.Confined;
        else if(!toggle) Cursor.lockState = CursorLockMode.Locked;
    }
}

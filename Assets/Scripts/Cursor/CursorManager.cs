using UnityEngine;

public static class CursorManager
{
    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Cursor has been locked");
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Cursor has been unlocked");
    }
}

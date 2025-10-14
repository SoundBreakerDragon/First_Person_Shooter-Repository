using UnityEngine;

public static class PauseManager
{
    public static bool paused = false;
    public static event DelegateTypes.VoidBool pauseToggle;

    public static void Reset()
    {
        paused = false;
        pauseToggle = null;
        Time.timeScale = 1f;
    }
    public static void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        //Run every pauseListener's togglePause function
        pauseToggle?.Invoke(paused); //? means only run the rest of
    }
}

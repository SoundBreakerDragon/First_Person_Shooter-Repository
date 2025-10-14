using UnityEngine;

public class PauseListener
{
    public bool paused = false;

    public PauseListener()
    {
        PauseManager.pauseToggle += TogglePause;

        this.paused = PauseManager.paused;
    }

    public void Destroy()
    {
        PauseManager.pauseToggle -= TogglePause; //If this class instance gets deleted
        //pauseToggle will not try to run a function on a deleted object.
    }

    public void TogglePause(bool paused)
    {
        this.paused = paused;
    }
}

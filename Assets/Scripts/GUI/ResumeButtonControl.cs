using UnityEngine;

public class ResumeButtonControl : MonoBehaviour
{
    public PauseMenuActivator pauseMenuActivator;

    public void ResumeGame()
    {
        if (pauseMenuActivator != null)
        {
            pauseMenuActivator.Resume();
        }
    }
}

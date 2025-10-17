using UnityEngine;

public class PauseMenuActivator : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool pauseableScene = false;
    bool stateUpdated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PauseManager.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseableScene)
        {
            CheckForPause();
        }
    }

    void CheckForPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePaused();
        }

        if(CanOpenPauseMenu())
        {
            OpenPauseMenu();
        }
        else if (CanClosePauseMenu())
        {
            ClosePauseMenu();
        }
    }

    void TogglePaused()
    {
        PauseManager.TogglePause();
        stateUpdated = true;
    }

    void OpenPauseMenu()
    {
        CursorManager.UnlockCursor();
        pauseMenu.SetActive(true);
        stateUpdated = false;

    }

    void ClosePauseMenu()
    {
        CursorManager.LockCursor();
        pauseMenu.SetActive(false);
        stateUpdated = false;
    }

    public void Resume()
    {
        stateUpdated = false;
        if(PauseManager.paused)
        {
            PauseManager.TogglePause();
        }
        pauseMenu.SetActive(false);
    }

    #region Conditions

    bool CanOpenPauseMenu()
    {
        return stateUpdated && PauseManager.paused;
    }

    bool CanClosePauseMenu()
    {
        return stateUpdated && !PauseManager.paused;
    }

    #endregion
}

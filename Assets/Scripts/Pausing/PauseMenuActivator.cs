using UnityEngine;

public class PauseMenuActivator : MonoBehaviour
{
    public bool pauseableScene = false;
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
            PauseManager.TogglePause();
        }
    }
}

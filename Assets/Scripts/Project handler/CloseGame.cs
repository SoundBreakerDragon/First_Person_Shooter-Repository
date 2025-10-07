using UnityEngine;

public class CloseGame : MonoBehaviour
{
    public void ExitGame()
    {
        print("Exit game was called");
        Application.Quit();
    }
}

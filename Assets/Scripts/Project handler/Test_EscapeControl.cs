using UnityEngine;

public class Test_EscapeControl : MonoBehaviour
{
    public string mainMenuName = "";
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<SceneLoader>().loadScene(mainMenuName);
        }
    }
}

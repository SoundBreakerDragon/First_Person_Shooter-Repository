using UnityEngine;

public class Test_LevelExit : MonoBehaviour
{
    public string nextSceneName = "";
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GetComponent<SceneLoader>().loadScene(nextSceneName);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

[CreateAssetMenu()]
public class CursorSceneLoadManager : ScriptableObject
{
    public List<SceneAsset> startLocked = new List<SceneAsset>();
    public List<SceneAsset> startUnLocked = new List<SceneAsset>();

    private void OnEnable() //When the object is created
    {
        SceneManager.sceneLoaded += CheckCursorStartMode;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckCursorStartMode;
    }

    private void CheckCursorStartMode(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Checking cursor for scene");
        bool sceneFound = false;

        for(int i = 0; i < startUnLocked.Count; i++) //Unlocked
        {
            if (startUnLocked[i].name == scene.name)
            {
                CursorManager.UnlockCursor();
                sceneFound = true;
                break;
            }
        }
        if(!sceneFound)
        {
            for (int i = 0; i < startLocked.Count; i++) //Locked
            {
                if(startLocked[i].name == scene.name)
                {
                    CursorManager.LockCursor();
                    break;
                }
            }
        }

        
    }
}

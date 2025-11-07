using UnityEngine;

public class Test_PlayerEnterArea : MonoBehaviour
{
    public string testWord;
    private void OnTriggerEnter(Collider other)
    {
        print(testWord);
    }
}

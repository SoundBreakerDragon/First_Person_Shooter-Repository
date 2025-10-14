using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public GameObject menuToOpen;
    public GameObject menuToClose;

    public void SwitchMenu()
    {
        menuToOpen.SetActive(true);
        menuToClose.SetActive(false);
    }
}

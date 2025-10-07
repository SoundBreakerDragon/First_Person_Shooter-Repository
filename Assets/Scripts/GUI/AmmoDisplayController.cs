using UnityEngine;
using TMPro;

public class AmmoDisplayController : MonoBehaviour
{
    public TMP_Text ammoText;
    string gunName = "";
    string currentAmmo = "";

    public void UpdateGunName(string gunName)
    {
        this.gunName = gunName;
        UpdateDisplay();
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        this.currentAmmo = $"{currentAmmo}/{maxAmmo}";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        ammoText.text = $"{gunName} {currentAmmo}";
    }
}

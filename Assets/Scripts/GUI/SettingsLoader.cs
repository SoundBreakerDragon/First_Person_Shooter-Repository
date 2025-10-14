using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsLoader : MonoBehaviour
{
    public SettingsDataManager settingsDataManager;
    public Slider volumeSlider;
    public Slider brightnessSlider;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) //For loading the build
        {
            settingsDataManager.Load();
        }
        volumeSlider.SetValueWithoutNotify(settingsDataManager.settingsData.volumePercentage);
        brightnessSlider.SetValueWithoutNotify(settingsDataManager.settingsData.brightnessPercentage);

    }
}

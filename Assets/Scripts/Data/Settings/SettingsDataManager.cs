using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu()]
public class SettingsDataManager : ScriptableObject
{
    public SettingData settingsData = new SettingData();
    [Header("Brightness")]
    public PostProcessProfile brightnessProfile;
    AutoExposure exposure;
    float buffer = 0.05f;

    string filePath = "game_settings.dat";
    bool loaded = false;

    private void OnEnable() //Runs when your project is loaded up or when your build is loaded up
    {
        Load();
    }

    public void ReceiveFloatSetting(FloatSetting setting, float value)
    {
        switch(setting)
        {
            case FloatSetting.Brightness:
                UpdateBrightness(value);
                break;

            case FloatSetting.MasterVolume:

                break;
        }
    }

    void UpdateBrightness(float value)
    {
        settingsData.brightnessPercentage = value;
        if(brightnessProfile == null)
        {
            Debug.LogWarning("Brightness Effect Post Processing Profile has not been asigned in Settings Data Manager. Cannot Update Brightness");
            return;
        }

        brightnessProfile.TryGetSettings(out exposure);
        if(exposure == null)
        {
            Debug.LogWarning("Exposure Effect could not be found on Brightness Post Processing Profile. Cannot Update Brightness");
            return;
        }
        ApplyBrightness();
    }

    void ApplyBrightness()
    {
        if(settingsData.brightnessPercentage >= buffer)
        {
            exposure.keyValue.value = settingsData.brightnessPercentage;
        }
        else
        {
            exposure.keyValue.value = buffer;
        }
    }

    #region File Management

    public void Save()
    {
        FileSaver.Save(settingsData, filePath);
    }

    public void Load()
    {
        if (loaded == false)
        {
            return;
        }
        else
        {
            loaded = true;
        }
        settingsData = (SettingData)FileSaver.Load(filePath);
        UpdateAllSettings();
    }

    void UpdateAllSettings()
    {
        UpdateBrightness(settingsData.brightnessPercentage);
    }

    #endregion
}

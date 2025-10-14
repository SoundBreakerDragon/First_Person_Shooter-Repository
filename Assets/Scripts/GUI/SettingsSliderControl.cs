using UnityEngine;

public class SettingsSliderControl : MonoBehaviour
{
    public SettingsDataManager settingsDataManager;
    public FloatSetting settingToPass;

    public void SliderUpdate (float value)
    {
        settingsDataManager.ReceiveFloatSetting(settingToPass, value);
    }

}

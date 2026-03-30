using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingManager
{
    private static SettingManager _instance;
    public static SettingManager Instance => _instance ?? (_instance = new SettingManager());
    private SettingManager()
    {
        settingData = DataManager.Instance.LoadSettingData(settingPath);
        if(settingData == null)
        {
            Debug.LogError($"设置数据加载异常，数据为空");
            settingData = new SettingData();
            DataManager.Instance.SaveSettingData(settingPath, settingData);
            Debug.Log("写入默认设置数据");
            return;
        }
        Application.targetFrameRate = 60;
    }
    //private EventType[] SettingKeys = { EventType.Track_1,EventType.Track_2,EventType.Track_3,EventType.Track_4 };
    public SettingData settingData { get; private set; }
    private const string settingPath = "SettingData";
    public void ChangeKey(EventType eventType,UnityAction<KeyCode> callback = null)
    {
        callback += (key) =>
        {
            InputManager.Instance.SetKeyInput(eventType, key);
            InputManager.Instance.StopCheck();
            //settingData.keySetting = InputManager.Instance.keyInputDic;
            Save();
        };
        InputManager.Instance.StartCheck(callback);
    }
    public void ResetSetting()
    {
        //if (settingData.keySetting != null) InputManager.Instance.ClearKeyInputDic();
        settingData = new SettingData();
        InputManager.Instance.SetKeyInputDic(settingData.keySetting);
        Save();
    }
    public void SetUseBloom(bool useBloom)
    {
        settingData.isUseBloom = useBloom;
        Save();
    }
    public void SetMusicVolume(float volume)
    {
        settingData.musicVolume = volume;
        if(AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(volume);
        Save();
    }
    
    private void Save()
    {
        DataManager.Instance.SaveSettingData(settingPath, settingData);
        EventBus.Instance.TriggerEvent(EventType.Update_SettingData);
    }
}
public abstract class BaseSettingInfo 
{
    public string title;
    public UnityAction updateDisplay;
    public abstract object GetValue();
    public abstract void OnValueChanged(object value);
}
public class ButtonSettingInfo : BaseSettingInfo
{
    public UnityAction callback;
    public string infoText;
    public ButtonSettingInfo(string title)
    {
        this.title = title;
    }

    public override object GetValue() => infoText;

    public override void OnValueChanged(object value) => callback?.Invoke();
}
public class SliderSettingInfo : BaseSettingInfo
{
    public float value;
    public UnityAction<float> onValueChanged;

    public override object GetValue() => value;

    public override void OnValueChanged(object value) => onValueChanged?.Invoke((float)value);
    public SliderSettingInfo(float value, string title)
    {
        this.value = value;
        this.title = title;
    }
}
public class ToggleSettingInfo : BaseSettingInfo
{
    public bool value;
    public UnityAction<bool> onValueChanged;
    public override object GetValue() => value;
    public override void OnValueChanged(object value) => onValueChanged?.Invoke((bool)value);
    public ToggleSettingInfo(bool value,string title)
    {
        this.value= value;
        this.title= title;
    }
}

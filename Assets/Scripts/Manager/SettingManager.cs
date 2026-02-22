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
    }
    public bool isUseBloom {  get; private set; } = true;
    public Dictionary<EventType, KeyCode> keySettingDic { get; private set; }
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
        if (settingData.keySetting != null) InputManager.Instance.ClearKeyInputDic();
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
        AudioManager.Instance.SetMusicVolume(volume);
        Save();
    }
    public void StartSetting()
    {
        Queue<BaseSettingInfo> infos = new Queue<BaseSettingInfo>();
        infos.Enqueue(new SettingInfo<float>(settingData.musicVolume,"音乐音量",
                                             SettingInfoDisPlayType.Text_Slider,(value)=>
        {
            SetMusicVolume(value);
        }));
        infos.Enqueue(new SettingInfo<bool>(settingData.isUseBloom,"Bloom",
                                            SettingInfoDisPlayType.Text_Toggle,(value) =>
        {
            SetUseBloom(value);
        }));
        foreach(var item in settingData.keySetting)
        {
            SettingInfo info = new SettingInfo();
            info.title = item.Key.ToString();
            info.infoText = item.Value.ToString();
            info.callback = () =>
            {
                ChangeKey(item.Key,(key)=>
                {
                    info.infoText = key.ToString();
                    info.updateText?.Invoke();
                });
                //if (InputManager.Instance.isCheckInput) 
                //else info.infoText = item.Value.ToString();
                info.infoText = "请输入";
            };
            infos.Enqueue(info);
        }
        UIManager.Instance.ShowUI<SettingPanel>();
        SettingPanel panel = UIManager.Instance.GetUI<SettingPanel>();
        panel.DisplaySettingInfo(infos);
    }
    private void Save()
    {
        DataManager.Instance.SaveSettingData(settingPath, settingData);
    }
}
public enum SettingInfoDisPlayType
{
    Text_Button,
    Text_Slider,
    Text_Toggle,
}
public abstract class BaseSettingInfo 
{
    public string title;
    public SettingInfoDisPlayType type;
}
public class SettingInfo<T> : BaseSettingInfo
{
    public T value;
    public UnityAction<T> callback;
    public SettingInfo(T value,string title, SettingInfoDisPlayType type, UnityAction<T> action)
    {
        this.title = title;
        this.type = type;
        this.callback = action;
        this.value = value;
    }
}
public class SettingInfo : BaseSettingInfo
{
    public UnityAction callback;
    public string infoText;
    public UnityAction updateText;
}

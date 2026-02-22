using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    private static SettingManager _instance;
    public static SettingManager Instance => _instance ?? (_instance = new SettingManager());
    private SettingManager()
    {
        SettingData data = DataManager.Instance.LoadSettingData(settingPath);
        if(data == null)
        {
            Debug.LogError($"设置数据加载异常，数据为空");
            data= new SettingData();
            keySettingDic = new Dictionary<EventType, KeyCode>();
            keySettingDic.Add(EventType.Track_1, KeyCode.A);
            keySettingDic.Add(EventType.Track_2, KeyCode.S);
            keySettingDic.Add(EventType.Track_3, KeyCode.K);
            keySettingDic.Add(EventType.Track_4, KeyCode.L);
            data.keySetting = keySettingDic;
            DataManager.Instance.SaveSettingData(settingPath, data);
            Debug.Log("写入设置数据");
            return;
        }
        keySettingDic = data.keySetting;
        isUseBloom = data.isUseBloom;
    }
    public bool isUseBloom {  get; private set; } = true;
    public Dictionary<EventType, KeyCode> keySettingDic { get; private set; }
    //private EventType[] SettingKeys = { EventType.Track_1,EventType.Track_2,EventType.Track_3,EventType.Track_4 };
    private const string settingPath = "SettingData";
    public void ChangeKey(EventType eventType)
    {
        InputManager.Instance.StartCheck((key) =>
        {
            InputManager.Instance.SetKeyInput(eventType, key);
            InputManager.Instance.StopCheck();
        });
    }
    public void ResetKeySetting()
    {
        if(keySettingDic != null) InputManager.Instance.ClearKeyInputDic();
        else keySettingDic = new Dictionary<EventType,KeyCode>();
        InputManager.Instance.SetKeyInput(EventType.Track_1, KeyCode.A);
        InputManager.Instance.SetKeyInput(EventType.Track_2, KeyCode.S);
        InputManager.Instance.SetKeyInput(EventType.Track_3, KeyCode.K);
        InputManager.Instance.SetKeyInput(EventType.Track_4, KeyCode.L);
    }
    public void SetUseBloom(bool useBloom)
    {
        isUseBloom = useBloom;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingPanel : BaseUI
{
    public Button butQuit;
    public Button butQuitGame;
    public Button butResetSetting;
    public Transform content;
    private const string settingItemPath = "UI/SettingItem";
    protected override void InitUI()
    {
        if(butQuit != null)
            butQuit.onClick.AddListener(Quit);
        if(butQuitGame != null)
            butQuitGame.onClick.AddListener(QuitGame);
        if(butResetSetting != null)
            butResetSetting.onClick.AddListener(ResetSetting);
    }
    public override void Show(UnityAction callback = null)
    {
        base.Show(callback);
        DisplaySettingInfo();
        UpdateSettingDataDisplay();
    }
    private void Quit()
    {
        ObjectPool.Instance.ClearPool();
        UIManager.Instance.HideUI<SettingPanel>(()=>
        {
            UIManager.Instance.ShowUI<SelectLevelPanel>();
        });
    }
    private void QuitGame()
    {
        UIManager.Instance.HideUI<SettingPanel>(() =>
        {
            Application.Quit();
        });
    }
    private void ResetSetting()
    {
        SettingManager.Instance.ResetSetting();
    }
    public void DisplaySettingInfo()
    {
        SettingData data = SettingManager.Instance.settingData;
        InitItem_musicVolume(data);
        InitItem_isUseBloom(data);
        if(Platform.IsPCPlatform()) InitItem_keySetting(data);
    }
    private void InitItem_isUseBloom(SettingData data)
    {
        ToggleSettingInfo info = new ToggleSettingInfo(data.isUseBloom,"Bloom");
        info.onValueChanged = (value) =>
        {
            SettingManager.Instance.SetUseBloom(value);
        };
        info.updateDisplay = () =>
        {
            info.value = SettingManager.Instance.settingData.isUseBloom;
        };
        CreateItem(info);
    }
    private void InitItem_musicVolume(SettingData data)
    {
        SliderSettingInfo info = new SliderSettingInfo(data.musicVolume,"ÒôÀÖÒôÁ¿");
        info.onValueChanged = (value) =>
        {
            SettingManager.Instance.SetMusicVolume(value);
        };
        info.updateDisplay = () =>
        {
            info.value = SettingManager.Instance.settingData.musicVolume;
        };
        CreateItem(info);
    }
    private void InitItem_keySetting(SettingData data)
    {
        foreach (var item in data.keySetting)
        {
            ButtonSettingInfo info = new ButtonSettingInfo(item.Key.ToString());
            info.infoText = item.Value.ToString();
            info.callback = () =>
            {
                SettingManager.Instance.ChangeKey(item.Key);
                info.infoText = "ÇëÊäÈë";
            };
            info.updateDisplay = () =>
            {
                info.infoText = SettingManager.Instance.settingData.keySetting[item.Key].ToString();
            };
            CreateItem(info);
        }
    }
    private void UpdateSettingDataDisplay()
    {
        EventBus.Instance.TriggerEvent(EventType.Update_SettingData);
    }
    private void CreateItem(BaseSettingInfo settingInfo)
    {
        if (settingInfo == null) return;
        GameObject obj = ObjectPool.Instance.GetObject(settingItemPath);
        obj.transform.SetParent(content, false);
        SettingItem item = obj.GetComponent<SettingItem>();
        item.settingInfo = settingInfo;
        item.Init();
    }
}

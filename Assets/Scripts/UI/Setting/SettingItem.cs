using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingItem : MonoBehaviour,IPoolItem
{
    public TextMeshProUGUI title;
    public Button butInfo;
    public TextMeshProUGUI butInfoText;
    public Slider sliderInfo;
    public Toggle toggleInfo;
    public BaseSettingInfo settingInfo;

    public ExtendType extendType { get; set; } = ExtendType.Extend;

    public event UnityAction Reset;
    private void Start()
    {
        if (title == null || butInfo == null ||
            sliderInfo == null || toggleInfo == null ||
            butInfoText == null)
        {
            Debug.LogError("存在空控件");
        }
    }
    public void Init()
    {
        if (settingInfo == null)
        {
            Debug.LogError($"传入SettingInfo为空");
            return;
        }
        else title.text = settingInfo.title;
        switch (settingInfo.type)
        {
            case SettingInfoDisPlayType.Text_Button:
                butInfo.gameObject.SetActive(true);
                butInfo.onClick.AddListener(ButInfo);
                UpdateButInfoText();
                break;
            case SettingInfoDisPlayType.Text_Slider:
                sliderInfo.gameObject.SetActive(true);
                sliderInfo.onValueChanged.AddListener(UpdateInfo<float>);
                sliderInfo.value = InitInfo<float>();
                break;
            case SettingInfoDisPlayType.Text_Toggle:
                toggleInfo.gameObject.SetActive(true);
                toggleInfo.onValueChanged?.AddListener(UpdateInfo<bool>);
                toggleInfo.isOn = InitInfo<bool>();
                break;
        }
    }
    private void ButInfo()
    {
        if (settingInfo is SettingInfo)
        {
            SettingInfo info = settingInfo as SettingInfo;
            info.callback?.Invoke();
            butInfoText.text = info.infoText;
            info.updateText = UpdateButInfoText;
        }
    }
    private void UpdateButInfoText()
    {
        if (settingInfo is SettingInfo)
        {
            SettingInfo info = settingInfo as SettingInfo;
            butInfoText.text = info.infoText;
        }
    }
    private T InitInfo<T>()
    {
        if (settingInfo is SettingInfo<T>)
            return (settingInfo as SettingInfo<T>).value;
        else
            return default(T);
    }
    private void UpdateInfo<T>(T value)
    {
        if(settingInfo is SettingInfo<T>)
            (settingInfo as SettingInfo<T>).callback?.Invoke(value);
    }
    public void OnReset()
    {
        if(settingInfo is SettingInfo) (settingInfo as SettingInfo).updateText = null;
        butInfo.gameObject.SetActive(false);
        sliderInfo.gameObject.SetActive(false);
        toggleInfo.gameObject.SetActive(false);
        butInfo.onClick.RemoveAllListeners();
        sliderInfo.onValueChanged.RemoveAllListeners();
        toggleInfo.onValueChanged.RemoveAllListeners();
        settingInfo = null;
        Reset?.Invoke();
        Reset = null;
    }
    private void OnDestroy()
    {
        if (settingInfo is SettingInfo) (settingInfo as SettingInfo).updateText = null;
        butInfo.onClick.RemoveAllListeners();
        sliderInfo.onValueChanged.RemoveAllListeners();
        toggleInfo.onValueChanged.RemoveAllListeners();
        settingInfo = null;
    }
}

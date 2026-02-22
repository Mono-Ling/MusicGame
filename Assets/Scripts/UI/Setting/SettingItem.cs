using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
    private Dictionary<Type, UIBehaviour> infoDic = new();
    private void Awake()
    {
        if (title == null || butInfo == null ||
            sliderInfo == null || toggleInfo == null ||
            butInfoText == null)
        {
            Debug.LogError("存在空控件");
        }
        infoDic.Add(typeof(ButtonSettingInfo), butInfo);
        infoDic.Add(typeof(SliderSettingInfo), sliderInfo);
        infoDic.Add(typeof(ToggleSettingInfo), toggleInfo);
    }
    //private void Start()
    //{
        
    //}
    public void Init()
    {
        if (settingInfo == null)
        {
            Debug.LogError($"传入SettingInfo为空");
            return;
        }
        else title.text = settingInfo.title;
        Type type = settingInfo.GetType();
        infoDic[type].gameObject.SetActive(true);
        BindEvent(type);
        EventBus.Instance.AddListener(EventType.Update_SettingData, UpdateInfoDisplay);
    }
    private void BindEvent(Type type)
    {
        if(type == typeof(ButtonSettingInfo))
        {
            butInfo.onClick.AddListener(()=>
            {
                settingInfo.OnValueChanged(null);
                butInfoText.text = settingInfo.GetValue().ToString();
            });
        }
        else if(type == typeof(SliderSettingInfo))
        {
            sliderInfo.onValueChanged.AddListener(value=>settingInfo.OnValueChanged(value));
        }
        else if (type == typeof(ToggleSettingInfo))
        {
            toggleInfo.onValueChanged.AddListener(value=> settingInfo.OnValueChanged(value));
        }
    }
    private void UpdateInfoDisplay()
    {
        settingInfo.updateDisplay?.Invoke();
        Type type = settingInfo.GetType();
        if (type == typeof(ButtonSettingInfo))
        {
            butInfoText.text = settingInfo.GetValue().ToString();
        }
        else if (type == typeof(SliderSettingInfo))
        {
            sliderInfo.value = (float)settingInfo.GetValue();
        }
        else if (type == typeof(ToggleSettingInfo))
        {
            toggleInfo.isOn = (bool)settingInfo.GetValue();
        }
    }
    private void ClearAction()
    {
        EventBus.Instance.RemoveListener(EventType.Update_SettingData, UpdateInfoDisplay);
        butInfo.onClick.RemoveAllListeners();
        sliderInfo.onValueChanged.RemoveAllListeners();
        toggleInfo.onValueChanged.RemoveAllListeners();
    }
    public void OnReset()
    {
        settingInfo = null;
        ClearAction();
        butInfo.gameObject.SetActive(false);
        sliderInfo.gameObject.SetActive(false);
        toggleInfo.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        ClearAction();
    }
}

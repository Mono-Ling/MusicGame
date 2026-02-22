using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
    public Dictionary<EventType,KeyCode> keySetting = new Dictionary<EventType,KeyCode>();
    //public Dictionary<string,KeyCode> keySaveSetting;
    public bool isUseBloom = true;
    public SettingData() { }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
    public Dictionary<EventType, KeyCode> keySetting = new Dictionary<EventType, KeyCode>
    {
        { EventType.Track_1, KeyCode.A },
        { EventType.Track_2, KeyCode.S },
        { EventType.Track_3, KeyCode.K },
        { EventType.Track_4, KeyCode.L }
    };
    public bool isUseBloom = true;
    public float musicVolume = 1f;
    public SettingData() { }
}

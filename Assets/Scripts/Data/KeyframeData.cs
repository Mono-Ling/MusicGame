using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct KeyframeData
{
    public float time;
    public float value;
    //public KeyframeData() { }
    public KeyframeData(float time, float value)
    {
        this.time = time;
        this.value = value;
    }
}

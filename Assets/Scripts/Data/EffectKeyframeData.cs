using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKeyframeData
{
    public float time;
    public float value;
    public EffectKeyframeData() { }
    public EffectKeyframeData(float time, float value)
    {
        this.time = time;
        this.value = value;
    }
}

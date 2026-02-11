using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public int trackId;
    public float startTime;
    public float hitTime;
    public float duration;
    public int unitType;
    public UnitData(int trackId, float time,float hitTime, int unitType, float duration)
    {
        this.trackId = trackId;
        this.startTime = time;
        this.hitTime = hitTime;
        this.duration = duration;
        this.unitType = unitType;
    }
    public UnitData()
    {
    }
}

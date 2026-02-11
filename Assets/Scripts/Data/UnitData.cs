using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public int trackId;
    public float startTime {  get; private set; }
    public float hitTime;
    public float duration;
    public int unitType;
    public UnitData(int trackId,float hitTime, int unitType, float duration)
    {
        this.trackId = trackId;
        this.hitTime = hitTime;
        this.duration = duration;
        this.unitType = unitType;
    }
    public UnitData()
    {
    }
    public void SetStartTime(float moveTime)
    {
        startTime = hitTime - moveTime;
    }
}

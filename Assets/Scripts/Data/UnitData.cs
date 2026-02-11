using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public int trackId;
    public float startTime;
    public float endTime;
    public int unitType;
    public UnitData(int trackId, float time,float hitTime, int unitType)
    {
        this.trackId = trackId;
        this.startTime = time;
        this.endTime = hitTime;
        this.unitType = unitType;
    }
    public UnitData()
    {
    }
}

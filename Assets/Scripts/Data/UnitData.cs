using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public int trackId;
    public float time;
    public float hitTime;
    public int unitType;
    public UnitData(int trackId, float time,float hitTime, int unitType)
    {
        this.trackId = trackId;
        this.time = time;
        this.hitTime = hitTime;
        this.unitType = unitType;
    }
    public UnitData()
    {
    }
}

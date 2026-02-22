using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForDSPTime : CustomYieldInstruction
{
    private double targetTime;
    public WaitForDSPTime(double seconds)
    {
        targetTime = GameTimeManager.Instance.GetGameTime() + seconds;
    }

    public override bool keepWaiting
    {
        get { return GameTimeManager.Instance.GetGameTime() < targetTime; }
    }
}

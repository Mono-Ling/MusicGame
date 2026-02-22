using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForDSPTime : CustomYieldInstruction
{
    private double targetTime;
    public WaitForDSPTime(double seconds)
    {
        targetTime = AudioSettings.dspTime + seconds;
    }

    public override bool keepWaiting
    {
        get { return AudioSettings.dspTime < targetTime; }
    }
}

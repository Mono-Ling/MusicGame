using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager
{
    private static GameTimeManager instance;
    public static GameTimeManager Instance => instance ?? (instance = new GameTimeManager());
    private double startTime;
    private double gameTime;
    private GameTimeManager() 
    {
        SetStartTime();
    }
    public void SetStartTime()
    {
        startTime = AudioSettings.dspTime;
    }
    public double GetGameTime()
    {
        gameTime = AudioSettings.dspTime - startTime;
        return gameTime;
    }
    public void PauseGame(bool isPause)
    {
        AudioListener.pause = isPause;
    }
}

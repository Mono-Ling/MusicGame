using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private static StateManager instance;
    public static StateManager Instance => instance ?? (instance = new StateManager());
    private StateManager() 
    { 
        isPlaying = false;
    }
    public bool isPlaying { get; private set; }
    public void GameStart()
    {
        isPlaying = true;
    }
}

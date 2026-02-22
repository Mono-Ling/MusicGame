using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public string name;
    public string musicPath;
    public string texturePath;
    public string unitDataPath;
    public string effectDataPath;
    public string moveTimeDataPath;
    public float time;
    public float maxMoveTime;
    public int level;
    public string description;
    public ColorData bkLowColor;
    public ColorData bkHighColor;
    public LevelData(string name, 
        string musicPath, string texturePath, string unitDataPath, string effectDataPath, string moveTimeDataPath,
        int level, float time, float maxMoveTime, string description,
        ColorData bkLowColor, ColorData bkHighColor)
    {
        this.name = name;
        this.musicPath = musicPath;
        this.texturePath = texturePath;
        this.unitDataPath = unitDataPath;
        this.effectDataPath = effectDataPath;
        this.moveTimeDataPath = moveTimeDataPath;
        this.time = time;
        this.maxMoveTime = maxMoveTime;
        this.level = level;
        this.description = description;
        this.bkLowColor = bkLowColor;
        this.bkHighColor = bkHighColor;
    }
    public LevelData() { }
}

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
    public float time;
    public int level;
    public string description;
    public LevelData(string name, string musicPath,string texturePath,string unitDataPath,string effectDataPath ,int level,float time,string description)
    {
        this.name = name;
        this.musicPath = musicPath;
        this.texturePath = texturePath;
        this.unitDataPath = unitDataPath;
        this.effectDataPath = effectDataPath;
        this.time = time;
        this.level = level;
        this.description = description;
    }
    public LevelData() { }
}

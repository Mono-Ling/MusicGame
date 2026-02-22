using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public string name;
    public string musicPath;
    public string texturePath;
    public string unitDataPath;
    public float time;
    public int level;
    public string description;
    public LevelData(string name, string musicPath,string texturePath,string unitDataPath,int level,float time,string description)
    {
        this.name = name;
        this.musicPath = musicPath;
        this.texturePath = texturePath;
        this.unitDataPath = unitDataPath;
        this.time = time;
        this.level = level;
        this.description = description;
    }
    public LevelData() { }
}

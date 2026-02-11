using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public string name;
    public string unitDataPath;
    public int level;
    public string description;
    public LevelData(string name,string unitDataPath,int level,string description)
    {
        this.name = name;
        this.unitDataPath = unitDataPath;
        this.level = level;
        this.description = description;
    }
    public LevelData() { }
}

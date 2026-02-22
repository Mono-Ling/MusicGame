using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelManager
{
    private static SelectLevelManager instance;
    public static SelectLevelManager Instance => instance ?? (instance = new SelectLevelManager());
    public LevelDataList levels { get; private set; }
    private List<UnitData> unitList;
    private AudioClip audio;
    private int levelIndex;
    private SelectLevelManager()
    {
        levels = DataManager.Instance.levelDataList;
        SetLevel(1);
    }
    public void SetLevel(int index)
    {
        if (index >= levels.levelList.Count)
        {
            Debug.LogWarning("关卡索引越界");
            index = levels.levelList.Count - 1;
        }
        else if (index < 0)
        {
            Debug.LogWarning("关卡索引越界");
            index = 0;
        }
        levelIndex = index;
        unitList = null;
        audio = null;
        GetLevelUnitDatas();
        GetLevelMusicClip();
    }
    public List<UnitData> GetLevelUnitDatas()
    {
        if(unitList != null) return unitList;
        unitList = DataManager.Instance.GetUnitList(levels.levelList[levelIndex]);
        return unitList;
    }
    public AudioClip GetLevelMusicClip()
    {
        if(audio != null) return audio;
        string path = levels.levelList[levelIndex].name;
        audio = Resources.Load<AudioClip>($"Music/{path}");
        if (audio == null) Debug.LogError($"路径{path}不存在");
        return audio;
    }
}

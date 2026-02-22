using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelManager
{
    private static SelectLevelManager instance;
    public static SelectLevelManager Instance => instance ?? (instance = new SelectLevelManager());
    public List<UnitData> unitList {  get; private set; }
    private LevelDataList levels;
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
            levelIndex = levels.levelList.Count - 1;
        }
        else if (index < 0)
        {
            Debug.LogWarning("关卡索引越界");
            levelIndex = 0;
        }
        levelIndex = index;
    }
    public List<UnitData> GetLevelUnitDatas()
    {
        return DataManager.Instance.GetUnitList(levels.levelList[levelIndex]);
    }
    public AudioClip GetLevelMusicClip()
    {
        string path = levels.levelList[levelIndex].name;
        AudioClip audio = Resources.Load<AudioClip>($"Music/{path}");
        if (audio == null) Debug.LogError($"路径{path}不存在");
        return audio;
    }
}

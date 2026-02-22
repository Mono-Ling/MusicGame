using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectLevelManager
{
    private static SelectLevelManager instance;
    public static SelectLevelManager Instance => instance ?? (instance = new SelectLevelManager());
    public LevelDataList levels { get; private set; }
    private List<UnitData> unitList;
    private AudioClip audio;
    private List<KeyframeData> effectList;
    private List<KeyframeData> moeTimeList;
    private Sprite coverSprite;
    private string description;
    private int levelIndex;
    private SelectLevelManager()
    {
        levels = DataManager.Instance.levelDataList;
        //SetLevel(0);
    }
    public void SetLevel(int index)
    {
        if(levelIndex == index) return;
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
        effectList = null;
        moeTimeList = null;
        audio = null;
        coverSprite = null;
        description = null;
        //GetLevelUnitDatas();
        //GetLevelMusicClip();
        GetCoverSprite();
        GetDescription();
        Debug.Log($"当前选中{levelIndex}");
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
        string path = levels.levelList[levelIndex].musicPath;
        string filePath = Path.Combine("Music", path);
        audio = Resources.Load<AudioClip>(filePath);
        if (audio == null) Debug.LogError($"路径{path}不存在");
        return audio;
    }
    public List<KeyframeData> GetEffectKeyframeDatas()
    {
        if(effectList != null) return effectList;
        string path = levels.levelList[levelIndex].effectDataPath;
        effectList = DataManager.Instance.GetKeyframeList(path);
        if(effectList == null) Debug.Log("该场景不存在后处理");
        return effectList;
    }
    public List<KeyframeData> GetUnitMoveTimeKeyframeDatas()
    {
        if (moeTimeList != null) return moeTimeList;
        string path = levels.levelList[levelIndex].moveTimeDataPath;
        moeTimeList = DataManager.Instance.GetKeyframeList(path);
        if (moeTimeList == null) Debug.Log("该场景不存在音符移动变速");
        return moeTimeList;
    }
    public float GetUnitMaxMoveTime()
    {
        return levels.levelList[levelIndex].maxMoveTime;
    }
    public Sprite GetCoverSprite()
    {
        if(coverSprite != null) return coverSprite;
        string path = levels.levelList[levelIndex].texturePath;
        string filePath = Path.Combine("LevelTexture", path);
        coverSprite = Resources.Load<Sprite>(filePath);
        if (coverSprite == null) Debug.LogError($"路径{path}不存在");
        return coverSprite;
    }
    public string GetDescription()
    {
        if (description != null) return description;
        description = levels.levelList[levelIndex].description;
        return description;
    }
    public float GetTime()
    {
        return levels.levelList[levelIndex].time;
    }
    private void ClearSelectBuffer()
    {
        coverSprite = null;
        description = null;
        Debug.Log("选择阶段缓存释放完成");
    }
    public void ExitSelectPanel()
    {
        ClearSelectBuffer();
        GetLevelMusicClip();
        GetLevelUnitDatas();
        GetEffectKeyframeDatas();
        GetUnitMoveTimeKeyframeDatas();
    }
    public bool CheckLevelData()
    {
        return levels != null && levels.levelList!=null && levels.levelList.Count > 0;
    }
}

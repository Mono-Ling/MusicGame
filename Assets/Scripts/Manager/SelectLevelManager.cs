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
        audio = Resources.Load<AudioClip>($"Music/{path}");
        if (audio == null) Debug.LogError($"路径{path}不存在");
        return audio;
    }
    public Sprite GetCoverSprite()
    {
        if(coverSprite != null) return coverSprite;
        string path = levels.levelList[levelIndex].texturePath;
        coverSprite = Resources.Load<Sprite>($"LevelTexture/{path}");
        if (coverSprite == null) Debug.LogError($"路径{path}不存在");
        return coverSprite;
    }
    public string GetDescription()
    {
        if (description != null) return description;
        description = levels.levelList[levelIndex].description;
        return description;
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
    }
    public bool CheckLevelData()
    {
        return levels != null && levels.levelList!=null && levels.levelList.Count > 0;
    }
}

using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance => _instance ??= new DataManager();
    //public MusicUnitList unitDataList;
    public LevelDataList levelDataList {  get; private set; }
    const string levelDataPath = "LevelData.json";
    private DataManager()
    {
        levelDataList = LoadData<LevelDataList>($"/{levelDataPath}");
        if (levelDataList == null)
        {
            Debug.LogError("加载失败");
        }
        else
        {
            Debug.Log("加载成功");
            Debug.Log($"关卡数据数量：{levelDataList.levelList.Count}");
        }
    }
    private T LoadData<T>(string path)
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + path);
        return JsonMapper.ToObject<T>(json);
    }
    public List<UnitData> GetUnitList(LevelData levelData)
    {
        //LevelData levelData = levelDataList.levelList[levelIndex];
        MusicUnitList unitDataList = LoadData<MusicUnitList>($"/{levelData.unitDataPath}");
        if (unitDataList == null)
        {
            Debug.LogError("加载失败");
        }
        else
        {
            Debug.Log("加载成功");
            Debug.Log($"单位数据数量：{unitDataList.unitList.Count}");
        }
        return unitDataList.unitList;
    }
    public List<EffectKeyframeData> GetEffectKeyframeList(LevelData levelData)
    {
        if(levelData == null) return null;
        if (levelData.effectDataPath == null) return null;
        EffectDataList effect = LoadData<EffectDataList>($"/{levelData.effectDataPath}");
        if (effect == null)
        {
            Debug.LogError("加载失败");
        }
        else
        {
            Debug.Log("加载成功");
            Debug.Log($"特效关键帧数据数量：{effect.keyframeList.Count}");
        }
        return effect.keyframeList;
    }
}
public class  MusicUnitList
{
    public List<UnitData> unitList;
}
public class LevelDataList
{
    public List<LevelData> levelList;
}
public class EffectDataList
{
    public List<EffectKeyframeData> keyframeList;
}

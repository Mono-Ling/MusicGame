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
    private LevelDataList levelDataList;
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
    public List<UnitData> GetUnitList(int levelIndex)
    {
        LevelData levelData = levelDataList.levelList[levelIndex];
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
}
public class  MusicUnitList
{
    public List<UnitData> unitList;
}
public class LevelDataList
{
    public List<LevelData> levelList;
}

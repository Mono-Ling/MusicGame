using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance => _instance ??= new DataManager();
    public MusicUnitList unitDataList;
    private DataManager()
    {
        unitDataList = LoadData<MusicUnitList>("/MusicUnit.json");
        if (unitDataList == null)
        {
            Debug.LogError("加载失败");
        }
        else
        {
            Debug.Log("加载成功");
            Debug.Log($"单位数据数量：{unitDataList.unitList.Count}");
        }
    }
    private T LoadData<T>(string path)
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + path);
        return JsonMapper.ToObject<T>(json);
    }
}
public class  MusicUnitList
{
    public List<UnitData> unitList;
}

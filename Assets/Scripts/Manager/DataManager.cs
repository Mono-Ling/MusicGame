using LitJson;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance => _instance ??= new DataManager();
    //public MusicUnitList unitDataList;
    public LevelDataList levelDataList {  get; private set; }
    const string levelDataPath = "LevelData.json";
    private DataManager()
    {
        levelDataList = LoadData<LevelDataList>($"{levelDataPath}");
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
        string jsonContent = null;
        string filePath = GetStreamingAssetsPath(path);

        if (Application.platform == RuntimePlatform.Android)
        {
            // 阻塞式读取（不推荐，可能卡顿）
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            www.SendWebRequest();

            // 等待请求完成
            while (!www.isDone)
            {
                // 空循环等待
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"加载文件失败: {filePath} | 错误: {www.error}");
                www.Dispose();
                return default(T);
            }

            jsonContent = www.downloadHandler.text;
            www.Dispose();
        }
        else
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"文件不存在: {filePath}");
                return default(T);
            }
            jsonContent = File.ReadAllText(filePath);
        }

        try
        {
            return JsonMapper.ToObject<T>(jsonContent);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON解析失败: {path} | 错误: {e.Message}");
            return default(T);
        }
    }

    // 获取跨平台路径方法（同方案1）
    private string GetStreamingAssetsPath(string fileName)
    {
        string path = "";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = $"jar:file://{Application.dataPath}!/assets/{fileName}";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = $"{Application.dataPath}/Raw/{fileName}";
                break;
            default:
                path = Path.Combine(Application.streamingAssetsPath, fileName);
                break;
        }
        return path;
    }
    public List<UnitData> GetUnitList(LevelData levelData)
    {
        //LevelData levelData = levelDataList.levelList[levelIndex];
        MusicUnitList unitDataList = LoadData<MusicUnitList>($"{levelData.unitDataPath}");
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
    public List<KeyframeData> GetKeyframeList(string keyframePath)
    {
        if(keyframePath == null) return null;
        KeyframeDataList keyframe = LoadData<KeyframeDataList>($"{keyframePath}");
        if (keyframe == null)
        {
            Debug.LogError("加载失败");
        }
        else
        {
            Debug.Log("加载成功");
            Debug.Log($"关键帧数据数量：{keyframe.keyframeList.Count}");
        }
        return keyframe.keyframeList;
    }
    public SettingData LoadSettingData(string settingPath)
    {
        try
        {
            if (settingPath == null) return null;
            string json = File.ReadAllText(Application.persistentDataPath + $"/{settingPath}.json");
            SettingData data = JsonConvert.DeserializeObject<SettingData>(json);
            if (data == null) Debug.LogWarning($"{settingPath}加载为空");
            return data;
        }
        catch
        {
            Debug.LogError($"设置数据加载失败");
            return null;
        }
    }
    public void SaveSettingData(string settingPath, SettingData data)
    {
        try
        {
            // 1. 安全拼接路径，避免重复的/符号
            string fullPath = Path.Combine(Application.persistentDataPath, $"{settingPath}.json");

            // 2. 获取文件所在目录，并确保目录存在
            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 3. 将数据序列化为JSON（使用Unity内置的JsonUtility，兼容性更好）
            string json = JsonConvert.SerializeObject(data); // true表示格式化输出，便于查看

            // 4. 写入文件
            File.WriteAllText(fullPath, json, System.Text.Encoding.UTF8);

            // 5. 输出成功日志，便于调试
            Debug.Log($"设置数据保存成功，路径：{fullPath}");
        }
        // 捕获具体异常，便于定位问题
        catch (System.ArgumentNullException ex)
        {
            Debug.LogError($"设置数据保存失败：路径为空 - {ex.Message}");
        }
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
public class KeyframeDataList
{
    public List<KeyframeData> keyframeList;
}

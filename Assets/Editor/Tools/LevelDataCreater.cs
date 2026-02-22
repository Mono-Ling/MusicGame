using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelDataCreater : EditorWindow
{
    [MenuItem("工具/关卡配置文件")]
    private static void ShowWindow()
    {
        LevelDataCreater window = EditorWindow.GetWindow<LevelDataCreater>("关卡配置文件生成工具");
        window.Show();
    }
    private List<LevelData> levels;
    private int addIndex;
    private int removeIndex;
    private Vector2 scrolPos;
    private void OnEnable()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/LevelData.json");
        var datas = JsonMapper.ToObject<LevelDataList>(json);
        levels = datas.levelList;
        if (levels == null) Debug.LogError("关卡数据为空");
        removeIndex = levels.Count - 1;
        addIndex = levels.Count-1;
    }
    private void OnGUI()
    {
        if(levels == null) return;
        Tool();
        EditorGUILayout.Space(20);
        DisplayLevelList();
    }
    private void Tool()
    {
        EditorGUILayout.BeginHorizontal();
        addIndex = EditorGUILayout.IntField("添加索引", addIndex);
        if (GUILayout.Button("添加"))
        {
            if (addIndex >= levels.Count - 1)
                levels.Add(new LevelData());
            else if (addIndex >= 0)
                levels.Insert(addIndex, new LevelData());
            else
                levels.Insert(0, new LevelData());
            addIndex = levels.Count - 1;
        }
        EditorGUILayout.Space();
        removeIndex = EditorGUILayout.IntField("移除索引", removeIndex);
        if (removeIndex < levels.Count && removeIndex >= 0 && GUILayout.Button("移除"))
        {
            levels.RemoveAt(removeIndex);
            removeIndex = levels.Count - 1;
        }
        EditorGUILayout.Space();
        if (levels != null && GUILayout.Button("保存"))
            Save(levels);
        EditorGUILayout.EndHorizontal();
    }
    private void DisplayLevelList()
    {
        scrolPos = EditorGUILayout.BeginScrollView(scrolPos);
        for (int i = 0; i < levels.Count; i++)
        {
            DisplayLevelData(i, levels[i]);
            EditorGUILayout.Space(30);
        }
        EditorGUILayout.EndScrollView();
    }
    private void DisplayLevelData(int index,LevelData level)
    {
        //EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"索引{index}");
        EditorGUILayout.Space(20);
        level.name = EditorGUILayout.TextField("关卡名称",level.name);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        AudioClip audio = null;
        if (level.musicPath != null) audio = Resources.Load<AudioClip>($"Music/{level.musicPath}");
        audio = EditorGUILayout.ObjectField("音乐",audio, typeof(AudioClip), false) as AudioClip;
        string path = null;
        if (audio != null)
        {
            path = AssetDatabase.GetAssetPath(audio);
            if (path != null) level.musicPath = Path.GetFileNameWithoutExtension(path);
        }
        level.musicPath = EditorGUILayout.TextField(level.musicPath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        Sprite sprite = null;
        if (level.musicPath != null) sprite = Resources.Load<Sprite>($"LevelTexture/{level.texturePath}");
        sprite = EditorGUILayout.ObjectField("图片", sprite, typeof(Sprite), false) as Sprite;
        path = null;
        if (audio != null)
        {
            path = AssetDatabase.GetAssetPath(sprite);
            if (path != null) level.texturePath = Path.GetFileNameWithoutExtension(path);
        }
        level.texturePath = EditorGUILayout.TextField(level.texturePath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        level.unitDataPath = EditorGUILayout.TextField("音符配置文件路径",level.unitDataPath);
        EditorGUILayout.Space();
        level.effectDataPath = EditorGUILayout.TextField("特效配置文件路径", level.effectDataPath);
        EditorGUILayout.Space();
        level.moveTimeDataPath = EditorGUILayout.TextField("音符移动时间配置文件路径", level.moveTimeDataPath);
        EditorGUILayout.Space();
        level.maxMoveTime = EditorGUILayout.FloatField("音符移动最长时间", level.maxMoveTime);
        EditorGUILayout.Space();
        level.time = EditorGUILayout.FloatField("时长",level.time);
        EditorGUILayout.Space();
        level.level = EditorGUILayout.IntField("等级",level.level);
        EditorGUILayout.Space();
        level.description = EditorGUILayout.TextField("描述", level.description);
        EditorGUILayout.Space();
        ColorData lowColorData = level.bkLowColor;
        ColorData highColorData = level.bkHighColor;
        Color lowColor = new Color(lowColorData.r,lowColorData.g,lowColorData.b,lowColorData.a);
        Color hightColor = new Color(highColorData.r,highColorData.g,highColorData.b,highColorData.a);
        lowColor = EditorGUILayout.ColorField("低区颜色", lowColor);
        hightColor = EditorGUILayout.ColorField("高区颜色",hightColor);
        level.bkLowColor.SetColor(lowColor);
        level.bkHighColor.SetColor(hightColor);
        //EditorGUILayout.EndHorizontal();
    }
    private void Save(List<LevelData> levels)
    {
        LevelDataList datas = new LevelDataList();
        datas.levelList = levels;
        string json = JsonMapper.ToJson(datas);
        File.WriteAllText(Application.streamingAssetsPath + "/LevelData.json",json);
    }
}

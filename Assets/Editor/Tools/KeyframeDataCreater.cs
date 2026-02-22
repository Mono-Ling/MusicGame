using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class KeyframeDataCreater : EditorWindow
{
    public enum Type
    {
        None,
        Change,
        Create,
    }
    [MenuItem("工具/关键帧文件生成工具")]
    private static void ShowWindow()
    {
        KeyframeDataCreater window = GetWindow<KeyframeDataCreater>("关键帧文件生成工具");
        window.Show();
    }
    private List<KeyframeData> datas;
    private Type type;
    private string path;
    private int removeIndex = 0;
    private int addIndex = 0;
    private Vector2 scrolPos;
    private void OnEnable()
    {
        type = Type.None;
    }
    private void OnGUI()
    {
        path = EditorGUILayout.TextField("路径", path);
        if (type == Type.None)
        {
            type = (Type)EditorGUILayout.EnumPopup("选择生成类型", type);
            return;
        }
        datas = GetDataList(datas);
        if (datas == null) return;
        Tool();
        EditorGUILayout.Space(20);
        scrolPos = EditorGUILayout.BeginScrollView(scrolPos);
        for (int i = 0; i < datas.Count; i++)
        {
            DisplayEffectData(i, datas[i]);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }
    private void Tool()
    {
        EditorGUILayout.BeginHorizontal();
        addIndex = EditorGUILayout.IntField("添加索引", addIndex);
        if (GUILayout.Button("添加"))
        {
            if (addIndex >=datas.Count - 1)
                datas.Add(new KeyframeData());
            else if (addIndex >= 0)
                datas.Insert(addIndex, new KeyframeData());
            else
                datas.Insert(0, new KeyframeData());
            addIndex = datas.Count - 1;
            removeIndex = datas.Count - 1;
        }
        EditorGUILayout.Space();
        removeIndex = EditorGUILayout.IntField("移除索引", removeIndex);
        if (removeIndex < datas.Count && removeIndex >= 0 && GUILayout.Button("移除"))
        {
            datas.RemoveAt(removeIndex);
            removeIndex = datas.Count - 1;
            addIndex = datas.Count - 1;
        }
        EditorGUILayout.Space();
        if (path != null && datas != null && GUILayout.Button("保存"))
            Save(datas);
        EditorGUILayout.EndHorizontal();
    }
    private List<KeyframeData> GetDataList(List<KeyframeData> datas)
    {
        if (datas != null) return datas;
        switch (type)
        {
            case Type.Change:
                if (path != null)
                {
                    try
                    {
                        string json = File.ReadAllText(Application.streamingAssetsPath + $"/{path}.json");
                        datas = JsonMapper.ToObject<KeyframeDataList>(json).keyframeList;
                    }
                    catch
                    {
                        Debug.LogWarning("无效路径");
                    }
                }
                else EditorGUILayout.HelpBox("路径为空", MessageType.Warning);
                break;
            case Type.Create:
                datas = new List<KeyframeData>();
                break;
        }
        removeIndex = datas.Count - 1;
        return datas;
    }
    private void DisplayEffectData(int index,KeyframeData data)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"索引{index}");
        EditorGUILayout.Space();
        data.time = EditorGUILayout.FloatField("时间",data.time);
        EditorGUILayout.Space();
        data.value = EditorGUILayout.Slider("强度", data.value, 0, 1);
        GUILayout.EndHorizontal();
    }
    private void Save(List<KeyframeData> datas)
    {
        KeyframeDataList list = new KeyframeDataList();
        list.keyframeList = datas;
        string json = JsonMapper.ToJson(list);
        File.WriteAllText(Application.streamingAssetsPath + $"/{path}.json", json);
        AssetDatabase.Refresh();
    }
}

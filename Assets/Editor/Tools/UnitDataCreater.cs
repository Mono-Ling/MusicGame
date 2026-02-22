using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UnitDataCreater : EditorWindow
{
    public enum Type
    {
        None,
        Change,
        Create,
    }
    private List<UnitData> unitDatas;
    private Type type;
    private int removeIndex = 0;
    private Vector2 scrolPos;
    private string path;
    [MenuItem("工具/音符配置文件")]
    private static void ShowWindow()
    {
        UnitDataCreater window = EditorWindow.GetWindow<UnitDataCreater>("音符配置文件生成工具");
        window.Show();
    }
    private void OnEnable()
    {
        type = Type.None;
    }
    private void OnGUI()
    {
        path = EditorGUILayout.TextField("路径",path);
        if (type == Type.None)
        {
            type = (Type)EditorGUILayout.EnumPopup("选择生成类型", type);
            return;
        }
        unitDatas =  GetDataList(unitDatas);
        if (unitDatas == null) return;
        Tool();
        EditorGUILayout.Space(20);
        DisplayDataList();
    }
    private void Tool()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加"))
            unitDatas.Add(new UnitData());
        EditorGUILayout.Space();
        removeIndex = EditorGUILayout.IntField("移除索引", removeIndex);
        if (removeIndex < unitDatas.Count && unitDatas.Count >= 0 && GUILayout.Button("移除"))
        {
            unitDatas.RemoveAt(removeIndex);
            removeIndex = unitDatas.Count - 1;
        }
        EditorGUILayout.Space();
        if (path != null && unitDatas != null && GUILayout.Button("保存"))
            Save(unitDatas);
        EditorGUILayout.EndHorizontal();
    }
    private List<UnitData> GetDataList(List<UnitData> datas)
    {
        if(datas != null) return datas;
        switch (type)
        {
            case Type.Change:
                if(path != null)
                {
                    try
                    {
                        string json = File.ReadAllText(Application.streamingAssetsPath + $"/{path}.json");
                        datas = JsonMapper.ToObject<MusicUnitList>(json).unitList;
                    }
                    catch
                    {
                        Debug.LogWarning("无效路径");
                    }
                }
                else EditorGUILayout.HelpBox("路径为空",MessageType.Warning);
                break;
            case Type.Create:
                datas = new List<UnitData>();
                break;
        }
        removeIndex = datas.Count - 1;
        return datas;
    }
    private void DisplayDataList()
    {
        
        scrolPos = EditorGUILayout.BeginScrollView(scrolPos);
        for (int i = 0; i < unitDatas.Count; i++)
        {
            DisplayUnitData(i,unitDatas[i]);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }
    private void DisplayUnitData(int index ,UnitData unitData)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField($"索引{index}");
        EditorGUILayout.Space();
        unitData.trackId = EditorGUILayout.IntPopup("轨道",unitData.trackId,new string[] {"1","2","3","4"},
                                                                            new int[] {1,2,3,4});
        EditorGUILayout.Space();
        unitData.hitTime = EditorGUILayout.FloatField("击打时间",unitData.hitTime);
        EditorGUILayout.Space();
        unitData.unitType = EditorGUILayout.IntPopup("类型", unitData.unitType, new string[] { "1", "2"},
                                                                            new int[] { 1, 2});
        EditorGUILayout.Space();
        if (unitData.unitType == 2)
            unitData.duration = EditorGUILayout.FloatField("持续时间", unitData.duration);
        EditorGUILayout.EndHorizontal();        
    }
    private void Save(List<UnitData> datas)
    {
        MusicUnitList unitList = new MusicUnitList();
        unitList.unitList = datas;
        string json = JsonMapper.ToJson(unitList);
        File.WriteAllText(Application.streamingAssetsPath + $"/{path}.json", json);
        AssetDatabase.Refresh();
    }
}

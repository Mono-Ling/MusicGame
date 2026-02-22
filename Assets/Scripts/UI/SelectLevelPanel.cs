using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectLevelPanel : BaseUI
{
    public float half {  get; private set; }
    public float edgeHeight;
    public float itemHeight;
    public RectTransform rectTransform;
    public LayoutElement layoutElement;
    public GameObject levelItemObj;
    private List<LevelItem> levelItems = new List<LevelItem>();
    private const string itemPath = "UI/LevelItem";
    protected override void InitUI()
    {
        levelItemObj = Resources.Load<GameObject>(itemPath);
        List<LevelData> datas = SelectLevelManager.Instance.levels.levelList;
        if (layoutElement != null)
        {
            layoutElement.minHeight = itemHeight * datas.Count + edgeHeight * 2;
        }
        foreach (LevelData data in datas)
        {
            AddLevelItem(data);
        }
    }
    protected override void Update()
    {
        base.Update();
        half = transform .position.y;
        List<LevelData> datas = SelectLevelManager.Instance.levels.levelList;
    }
    private void AddLevelItem(LevelData data)
    {
        var obj = Instantiate(levelItemObj,rectTransform,false);
        LevelItem item = obj.GetComponent<LevelItem>();
        levelItems.Add(item);
        item.levelData = data;
        Debug.Log(levelItems.Count);
    }
    private LevelItem Select()
    {
        int left = 0;
        int right = levelItems.Count - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (levelItems[mid].transform .position.y < half)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        int targetIndex = Mathf.Max(0, right);
        return levelItems[targetIndex];
    }
}

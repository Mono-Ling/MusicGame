using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelPanel : BaseUI
{
    public float half {  get; private set; }
    public float edgeHeight;
    public float itemHeight;
    public RectTransform rectTransform;
    public LayoutElement layoutElement;
    public Button butStart;
    public Image cover;
    public Text description;
    public GameObject levelItemObj;
    private List<LevelItem> levelItems = new List<LevelItem>();
    private string itemPath = "UI/LevelItem";
    protected override void InitUI()
    {
        itemPath = Path.Combine("UI", "LevelItem");
        levelItemObj = Resources.Load<GameObject>(itemPath);
        List<LevelData> datas = SelectLevelManager.Instance.levels.levelList;
        if (layoutElement != null)
        {
            float height = Screen.height / edgeHeight;
            layoutElement.minHeight = 140 * datas.Count + height * 2;
        }
        foreach (LevelData data in datas)
        {
            AddLevelItem(data);
        }
        if(butStart == null)
        {
            Debug.LogError("开始按钮为空！");
            return;
        }
        butStart.onClick.AddListener(GameStart);
    }
    protected override void Update()
    {
        base.Update();
        half = transform .position.y;
        int levelIndex = Select();
        SelectLevelManager.Instance.SetLevel(levelIndex);
        DisplayCover();
        DisplayDescription();
    }
    private void AddLevelItem(LevelData data)
    {
        var obj = Instantiate(levelItemObj,rectTransform,false);
        LevelItem item = obj.GetComponent<LevelItem>();
        levelItems.Add(item);
        item.levelData = data;
        //Debug.Log(levelItems.Count);
    }
    private int Select()
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
        int targetIndex = Mathf.Min(left, levelItems.Count-1);
        //int targetIndex = Mathf.Max(0, right);
        return targetIndex;
    }
    private void DisplayCover()
    {
        if(cover == null)
        {
            Debug.LogError("UI封面图片为空");
            return;
        }
        cover.sprite = SelectLevelManager.Instance.GetCoverSprite();
    }
    private void DisplayDescription()
    {
        if(description == null)
        {
            Debug.LogError("UI描述文本组件为空");
            return;
        }
        description.text = SelectLevelManager.Instance.GetDescription();
    }
    private void GameStart()
    {
        UIManager.Instance.HideUI<SelectLevelPanel>(() =>
        {
            SelectLevelManager.Instance.ExitSelectPanel();
            SceneManager.LoadScene("Game");
        });
    }
}

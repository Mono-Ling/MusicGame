using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingPanel : BaseUI
{
    public Button butQuit;
    public Button butQuitGame;
    public Transform content;
    private const string settingItemPath = "UI/SettingItem";
    protected override void InitUI()
    {
        if(butQuit != null)
            butQuit.onClick.AddListener(Quit);
        if(butQuitGame != null)
            butQuitGame.onClick.AddListener(QuitGame);
    }
    private void Quit()
    {
        UIManager.Instance.HideUI<SettingPanel>(()=>
        {
            UIManager.Instance.ShowUI<SelectLevelPanel>();
        });
    }
    private void QuitGame()
    {
        UIManager.Instance.HideUI<SettingPanel>(() =>
        {
            Application.Quit();
        });
    }
    public void DisplaySettingInfo(Queue<BaseSettingInfo> infoQueue)
    {
        if(infoQueue == null) return;
        while (infoQueue.Count > 0)
        {
            GameObject obj = ObjectPool.Instance.GetObject(settingItemPath);
            obj.transform.SetParent(content,false);
            SettingItem item = obj.GetComponent<SettingItem>();
            item.settingInfo = infoQueue.Dequeue();
            item.Init();
        }
    }
}

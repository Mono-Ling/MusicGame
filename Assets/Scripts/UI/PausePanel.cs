using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : BaseUI
{
    public Button butQuit;
    public Button butContinue;

    protected override void InitUI()
    {
        if(butQuit != null)
            butQuit.onClick.AddListener(OnQuit);
        else Debug.LogError("PausePanel: butQuit未关联");
        if (butContinue != null)
            butContinue.onClick.AddListener(OnContinue);
        else Debug.LogError("PausePanel: butContinue未关联");
    }
    private void OnQuit()
    {
        UIManager.Instance.HideUI<PausePanel>(() =>
        {
            EventBus.Instance.TriggerEvent(EventType.EndGame);
            SceneManager.LoadScene("Begin");
            ObjectPool.Instance.ClearPool();
        });
    }
    private void OnContinue()
    {
        UIManager.Instance.HideUI<PausePanel>();
        EventBus.Instance.TriggerEvent(EventType.PauseGame);
    }
}

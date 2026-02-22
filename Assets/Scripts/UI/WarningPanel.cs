using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : BaseUI
{
    public string errorData = "ErrorData";
    public Text text;
    public Button butQuit;
    protected override void InitUI()
    {
        if (butQuit != null) butQuit.onClick.AddListener(Quit);
        else Debug.LogError("按钮未关联");
        if (text == null) Debug.LogError("文本未关联");
    }
    protected override void Update()
    {
        base.Update();
        text.text = errorData;
    }
    private void Quit()
    {
        Application.Quit();
    }
}

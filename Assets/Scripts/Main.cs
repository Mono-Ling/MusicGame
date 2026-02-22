using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SelectLevelManager.Instance.CheckLevelData()) UIManager.Instance.ShowUI<SelectLevelPanel>();
        else
        {
            UIManager.Instance.ShowUI<WarningPanel>();
            var warning = UIManager.Instance.GetUI<WarningPanel>();
            warning.errorData = "”Œœ∑≥ı ºªØ¥ÌŒÛ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

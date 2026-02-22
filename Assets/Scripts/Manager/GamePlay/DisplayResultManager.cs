using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultType
{
    None,
    Perfect,
    Great,
    Good,
    Miss,
}
public class DisplayResultManager : MonoBehaviour
{
    private static DisplayResultManager instance;
    public static DisplayResultManager Instance => instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            Debug.LogWarning("单例重复注册");
        }
        instance = this;
    }
    public float perfectWindow;
    public float greatWindow;
    public float goodWindow;
    public float fastScale;
    private List<BaseResultText> textList = new List<BaseResultText>();
    // Start is called before the first frame update
    void Start()
    {
        float spetTime = UnitManager.Instance.window / 3;
        perfectWindow = spetTime;
        greatWindow = spetTime * 2;
        goodWindow = spetTime * 3;
    }
    public void GetInputResult(float timeDiff)
    {
        timeDiff = Mathf.Abs(timeDiff);

        ResultType resultType = ResultType.None;
        if (timeDiff < perfectWindow) resultType = ResultType.Perfect;
        else if (timeDiff < greatWindow) resultType = ResultType.Great;
        else resultType = ResultType.Good;

        UpdateResult(resultType);
    }
    public void GetMissResult()
    {
        UpdateResult(ResultType.Miss);
        Debug.Log("【Miss】失效音符");
    }
    private void UpdateResult(ResultType resultType)
    {
        UpdateResultTextUI(resultType);
        EventBus.Instance.TriggerEvent<ResultType>(EventType.Update_InputResult, resultType);
    }
    private void UpdateResultTextUI(ResultType resultType)
    {
        BaseResultText newText = null;

        switch(resultType)
        {
            case ResultType.Perfect:
                newText = UIManager.Instance.DontBufferShowUI<PerfectText>(EndShow);
                break;
            case ResultType.Great:
                newText = UIManager.Instance.DontBufferShowUI<GreatText>(EndShow);
                break;
            case ResultType.Good:
                newText = UIManager.Instance.DontBufferShowUI<GoodText>(EndShow);
                break;
            case ResultType.Miss:
                break;
            default:
                break;
        }

        if (newText == null) return;
        UpdateList(textList);
        textList.Add(newText);
        newText.Reset += () =>
        {
            textList.Remove(newText);
            Debug.Log("提前回收");
        };
    }
    private void UpdateList(List<BaseResultText> list)
    {
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Fast(fastScale);
        }
    }
    private void EndShow()
    {
        BaseResultText text = null;
        if (textList.Count > 0)
        {
            text = textList[0];
            textList.RemoveAt(0);
        }
        UIManager.Instance.DontBufferHideUI(text);
    }
}

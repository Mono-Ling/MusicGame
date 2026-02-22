using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultType
{
    Perfect,
    Great,
    Good,
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
            Debug.LogWarning("µ¥ÀýÖØ¸´×¢²á");
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

    // Update is called once per frame
    void Update()
    {
    }
    public void AddTask(float time)
    {
        time = Mathf.Abs(time);
        BaseResultText newText = null;

        if (time < perfectWindow) newText = UIManager.Instance.DontBufferShowUI<PerfectText>(EndShow);
        else if (time < greatWindow) newText = UIManager.Instance.DontBufferShowUI<GreatText>(EndShow);
        else newText = UIManager.Instance.DontBufferShowUI<GoodText>(EndShow);

        if (newText == null) return;
        UpdateList(textList);
        textList.Add(newText);
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

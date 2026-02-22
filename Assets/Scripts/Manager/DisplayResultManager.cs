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
    private bool isTaskRunning = false;
    private BaseResultText[] texts;
    // Start is called before the first frame update
    void Start()
    {
        float spetTime = UnitManager.Instance.window / 3;
        perfectWindow = spetTime;
        greatWindow = spetTime * 2;
        goodWindow = spetTime * 3;
        texts = new BaseResultText[3];
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddTask(float time)
    {
        if (isTaskRunning)
        {
            if (texts[0] != null)
            {
                UIManager.Instance.HideUI<PerfectText>(null, false);
                texts[0] = null;
            }
            if(texts[1] != null)
            {
                UIManager.Instance.HideUI<GreatText>(null, false);
                texts[1] = null;
            }
            if (texts[2] != null)
            {
                UIManager.Instance.HideUI<GoodText>(null, false);
                texts[2] = null;
            }
        }
        time = Mathf.Abs(time);
        if (time < perfectWindow)
        {
            UIManager.Instance.ShowUI<PerfectText>(() => {
                UIManager.Instance.HideUI<PerfectText>(() =>
                {
                    isTaskRunning = false;
                    texts[0] = null;
                });
            });
            texts[0] = UIManager.Instance.GetUI<PerfectText>();
            isTaskRunning = true;
        }
        else if (time < greatWindow)
        {
            UIManager.Instance.ShowUI<GreatText>(() => {
                UIManager.Instance.HideUI<GreatText>(() =>
                {
                    isTaskRunning = false;
                    texts[1] = null;
                });
            });
            texts[1] = UIManager.Instance.GetUI<GreatText>();
            isTaskRunning= true;
        }
        else
        {
            UIManager.Instance.ShowUI<GoodText>(() => {
                UIManager.Instance.HideUI<GoodText>(() =>
                {
                    isTaskRunning = false;
                    texts[2] = null;
                });
            });
            texts[2] = UIManager.Instance.GetUI<GoodText>();
            isTaskRunning = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    public static EffectManager Instance => instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            Debug.LogWarning("单例重复注册");
        }
        instance = this;
    }
    public float bloomMaxValue;
    private Bloom bloom;
    private List<KeyframeData> keyframDatas;
    private Queue<KeyframeData> keyframeDataQueue = new Queue<KeyframeData>();
    // Start is called before the first frame update
    void Start()
    {
        bloom = Camera.main.GetComponent<Bloom>();
        keyframDatas = SelectLevelManager.Instance.GetEffectKeyframeDatas();
        if (bloom == null)
        {
            Debug.LogError("Bloom组件为空");
            enabled = false;
            return;
        }
        if (!SettingManager.Instance.settingData.isUseBloom)
        {
            bloom.enabled = false;
            enabled = false;
            Debug.Log("后处理效果关闭");
            return;
        }
        if (keyframDatas == null)
        {
            bloom.enabled = false;
            Debug.Log("关闭Bloom后处理");
            enabled = false;
            return;
        }
        foreach(KeyframeData data in keyframDatas)
        {
            keyframeDataQueue.Enqueue(data);
        }
        //InputManager.Instance.StartGame += () => 
        //{ 
        //    StartCoroutine(UpdateEffect());
        //    Debug.Log("后处理启动");
        //};
        EventBus.Instance.AddListener(EventType.StartGame,StartEffect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartEffect()
    {
        StartCoroutine(UpdateEffect());
        Debug.Log("后处理启动");
        EventBus.Instance.RemoveListener(EventType.StartGame, StartEffect);
    }
    IEnumerator UpdateEffect()
    {
        float currentValue = 0f;
        //Debug.Log($"{this}协程开启");
        while (keyframeDataQueue.Count > 0)
        {
            float startTime = (float)GameManager.Instance.currentTime;
            float endTime = keyframeDataQueue.Peek().time;
            float endValue = keyframeDataQueue.Peek().value;
            float startValue = currentValue;
            if (Mathf.Approximately(endTime, startTime))
            {
                currentValue = endValue;
                bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, bloomMaxValue, currentValue * bloomMaxValue);
                keyframeDataQueue.Dequeue();
                continue;
            }
            while (true)
            {
                float currentTime = (float)GameManager.Instance.currentTime;
                if (currentTime >= endTime)
                {
                    currentValue = endValue;
                    //float clampedValue = Mathf.Clamp(currentValue, 0, bloomMaxValue);
                    bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, bloomMaxValue, currentValue * bloomMaxValue);
                    break;
                }
                float t = (currentTime - startTime) / (endTime - startTime);
                t = Mathf.Clamp01(t);
                currentValue = Mathf.Lerp(startValue, endValue, t);
                bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, bloomMaxValue, currentValue * bloomMaxValue);
                yield return null;
            }
            keyframeDataQueue.Dequeue();
        }
        Debug.Log($"{this}协程结束");
    }
}

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
            Destroy(instance.gameObject);
            Debug.LogWarning("单例重复注册");
        }
        instance = this;
    }
    public float bloomMaxValue;
    private Bloom bloom;
    private List<EffectKeyframeData> keyframDatas;
    private Queue<EffectKeyframeData> keyframeDataQueue = new Queue<EffectKeyframeData>();
    // Start is called before the first frame update
    void Start()
    {
        bloom = Camera.main.GetComponent<Bloom>();
        keyframDatas = SelectLevelManager.Instance.GetEffectKeyframeDatas();
        if (bloom == null)
        {
            Debug.LogError("Bloom组件为空");
            enabled = false;
        }
        if (keyframDatas == null)
        {
            bloom.enabled = false;
            Debug.Log("关闭Bloom后处理");
            enabled = false;
            return;
        }
        foreach(EffectKeyframeData data in keyframDatas)
        {
            keyframeDataQueue.Enqueue(data);
        }
        InputManager.Instance.StartGame += () => 
        { 
            StartCoroutine(UpdateEffect());
            Debug.Log("后处理启动");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
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
                bloom.LThreshold = bloomMaxValue - currentValue;
                keyframeDataQueue.Dequeue();
                continue;
            }
            while (true)
            {
                float currentTime = (float)GameManager.Instance.currentTime;
                if (currentTime >= endTime)
                {
                    currentValue = endValue;
                    float clampedValue = Mathf.Clamp(currentValue, 0, bloomMaxValue);
                    bloom.LThreshold = bloomMaxValue - clampedValue;
                    break;
                }
                float t = (currentTime - startTime) / (endTime - startTime);
                t = Mathf.Clamp01(t);
                currentValue = Mathf.Lerp(startValue, endValue, t);
                bloom.LThreshold = bloomMaxValue - Mathf.Clamp(currentValue, 0, bloomMaxValue);
                yield return null;
            }
            keyframeDataQueue.Dequeue();
        }
        Debug.Log($"{this}协程结束");
    }
}

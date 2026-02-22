using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveEffObj : MonoBehaviour,IPoolItem
{
    public int maxNum = 4;
    public float delayTime = 0.5f;

    //public ExtendType extendType { get; set; } = ExtendType.Extend;

    public event UnityAction Reset;
    public void Init()
    {
        Invoke("Hide",delayTime);
    }
    private void Hide()
    {
        ObjectPool.Instance.PutObject(gameObject);
    }
    public void OnReset()
    {
        transform.position = Vector3.zero;
        Reset?.Invoke();
        Reset = null;
    }
    public int GetMaxNum()
    {
        return maxNum;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

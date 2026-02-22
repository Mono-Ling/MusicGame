using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance => instance ?? (instance = new ObjectPool());
    private Dictionary<string,PoolItem> poolDic = new Dictionary<string,PoolItem>();
    public GameObject GetObject(string name)
    {
        if (!poolDic.ContainsKey(name)) poolDic.Add(name, new PoolItem(name));
        IPoolItem item;
        GameObject obj = null;
        PoolItem poolItem = poolDic[name];
        if (poolItem.poolCount == 0 && poolItem.usedCount < poolItem.maxNum)
        {
            GameObject prefab = poolItem.prefab; //Resources.Load<GameObject>(name);
            if (prefab == null)
            {
                Debug.LogError($"{name}预设体不存在");
                return null;
            }
            obj = GameObject.Instantiate(prefab);
            obj.name = name;
            item = obj.GetComponent<IPoolItem>();
        }
        else
            item = poolItem.Get();
        //if (item != null) poolItem.AddUsed(item);
        //else Debug.LogError($"{name}不存在对象池接口");
        if (obj == null && item is Component component) obj = component.gameObject;
        obj?.SetActive(true);
        return obj;
    }
    public void PutObject(GameObject obj)
    {
        if(obj == null) return;
        string name = obj.name;
        obj.SetActive(false);
        if(!poolDic.ContainsKey(name)) return;
        PoolItem poolItem = poolDic[name];
        IPoolItem item = obj.GetComponent<IPoolItem>();
        item.OnReset();
        poolItem.Put(item);
    }
    public void ClearPool()
    {
        poolDic.Clear();
        Debug.Log("对象池清空");
    }
}
public class PoolItem
{
    private Queue<IPoolItem> objectPool = new Queue<IPoolItem>();
    private List<IPoolItem> usedItems = new List<IPoolItem>();
    public int maxNum {  get; private set; } = int.MaxValue;
    public int poolCount => objectPool.Count;
    public int usedCount => usedItems.Count;
    public GameObject prefab {  get; private set; }
    public IPoolItem Get()
    {
        IPoolItem item = null;
        if (objectPool.Count == 0 && usedCount > maxNum)
        {
            item = usedItems[0];
            usedItems.RemoveAt(0);
            //item.Init();
            item.OnReset();
            usedItems.Add(item);
            return item;
        }
       if(objectPool.Count > 0)  item = objectPool.Dequeue();
       if(item != null)  usedItems.Add(item);
        return item;
    }
    public void Put(IPoolItem item)
    {
        if (item == null) return;
        //item.Init();
        item.OnReset();
        objectPool.Enqueue(item);
        usedItems.Remove(item);
    }
    public void AddUsed(IPoolItem item)
    {
        if (item == null) return;
        usedItems.Add(item);
        maxNum = item.GetMaxNum();
    }
    public PoolItem(string name) 
    {
        prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
            Debug.LogError($"{name}预设体不存在");
    }
}
public interface IPoolItem
{
    event UnityAction Reset;
    //ExtendType extendType { get; set; }
    void Init();
    void OnReset();
    int GetMaxNum()
    {
        return int.MaxValue;
    }
}

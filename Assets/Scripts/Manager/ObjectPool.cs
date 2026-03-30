using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ExtendType
{
    Reuse,
    Extend,
}
public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance => instance ?? (instance = new ObjectPool());
    private Dictionary<string,PoolItem> poolDic = new Dictionary<string,PoolItem>();
    public GameObject GetObject(string name)
    {
        if (!poolDic.ContainsKey(name)) poolDic.Add(name, new PoolItem(name));
        GameObject obj = null;
        PoolItem poolItem = poolDic[name];
        IPoolItem item = poolItem.Get(name);
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
        //item.OnReset();
        poolItem.Put(obj);
    }
    public void ClearPool()
    {
        foreach(PoolItem poolItem in poolDic.Values)
        {
            poolItem.ClearPool();
        }
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
    public ExtendType extendType { get; private set; }
    public GameObject prefab {  get; private set; }
    /// <summary>
    /// 弃用
    /// </summary>
    /// <returns></returns>
    public IPoolItem Get()
    {
        IPoolItem item = null;
        if (objectPool.Count == 0 && usedCount > maxNum)
        {
            item = usedItems[0];
            usedItems.RemoveAt(0);
            //item.Init();
            item.OnReset();
            //usedItems.Add(item);
            //return item;
        }
       else if(objectPool.Count > 0)  item = objectPool.Dequeue();
       if(item != null)  usedItems.Add(item);
        return item;
    }
    public IPoolItem Get(string name)
    {
        IPoolItem item = null;
        if(poolCount > 0) item = objectPool.Dequeue();
        else
        {
            if (usedCount > maxNum)
            {
                switch (extendType)
                {
                    case ExtendType.Extend:
                        item = CreateObject(name);
                        break;
                    case ExtendType.Reuse:
                        item = usedItems[0];
                        usedItems.RemoveAt(0);
                        item.OnReset();
                        break;
                    default:
                        break;
                }
            }
            else item = CreateObject(name);
        }
        if (item != null) usedItems.Add(item);
        return item;
    }
    private IPoolItem CreateObject(string name)
    {
        IPoolItem item = null;
        if (prefab == null)
        {
            Debug.LogError($"{name}预设体为空");
            return null;
        }
        GameObject itemObj = GameObject.Instantiate(prefab);
        itemObj.name = name;
        item = itemObj.GetComponent<IPoolItem>();
        if (item == null) Debug.LogWarning($"{name}未继承对象池接口");
        return item;
    }
    public void Put(GameObject obj)
    {
        if (obj == null) return;
        //item.Init();
        IPoolItem item = obj.GetComponent<IPoolItem>();
        if (item != null) usedItems.Remove(item);
        else
        {
            Debug.LogError($"{obj}未实现对象池接口");
            return;
        }
        item.OnReset();
        if (extendType == ExtendType.Extend && poolCount >= maxNum)
            GameObject.Destroy(obj);
        else objectPool.Enqueue(item);
        //usedItems.Remove(item);
    }
    public void AddUsed(IPoolItem item)
    {
        if (item == null) return;
        usedItems.Add(item);
    }
    public void ClearPool()
    {
        foreach (var item in objectPool)
        {
            if (item is Component component) GameObject.Destroy(component.gameObject);
        }
        foreach (var item in usedItems)
        {
            try
            {
                if (item is Component component) GameObject.Destroy(component.gameObject);
            } catch { }
        }
        objectPool.Clear();
        usedItems.Clear();
    }
    public PoolItem(string name) 
    {
        prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
        {
            Debug.LogError($"{name}预设体不存在");
            return;
        }
        IPoolItem item = prefab.GetComponent<IPoolItem>();
        if(item == null)
        {
            Debug.LogError($"{prefab}未实现对象池接口");
            return;
        }
        maxNum = item.GetMaxNum();
        extendType = item.extendType;
    }
}
public interface IPoolItem
{
    event UnityAction Reset;
    ExtendType extendType { get; set; }
    void Init();
    void OnReset();
    int GetMaxNum() => int.MaxValue;
}

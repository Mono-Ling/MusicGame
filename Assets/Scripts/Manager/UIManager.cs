using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    private static UIManager instance;
    public static UIManager Instance => instance ?? (instance = new UIManager());
    //private const string canvasPath = "UI/Canvas";
    private Transform canvasTransform;
    private Dictionary<string,BaseUI> uiDic = new Dictionary<string,BaseUI>();
    private UIManager() 
    {
        string canvasPath = Path.Combine("UI","Canvas");
        var canvas = GameObject.Instantiate(Resources.Load<GameObject>(canvasPath));
        if (canvas == null)
        {
            Debug.LogError("Canvas加载失败");
            return;
        }
        canvasTransform = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
    }
    /// <summary>
    /// 获取UI（预设体名须与类名一至）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUI<T>() where T : BaseUI
    {
        string name = typeof(T).Name;
        if (uiDic.ContainsKey(name))
            return uiDic[name] as T;
        Debug.LogWarning($"没有找到UI：{name}");
        return null;
    }
    /// <summary>
    /// 显示UI（预设体名须与类名一至）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callback"></param>
    public void ShowUI<T>(UnityAction callback = null) where T : BaseUI
    {
        string name = typeof (T).Name;
        if(uiDic.ContainsKey(name))
        {
            Debug.LogWarning($"{name}已打开");
            return;
        }
        string filePath = Path.Combine("UI", name);
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(filePath),canvasTransform,false);
        if (obj == null)
        {
            Debug.LogError($"{name}打开失败");
            return;
        }
        T ui = obj.GetComponent<T>();
        uiDic.Add(name, ui);
        ui.Show(callback);
    }
    /// <summary>
    /// 隐藏UI（预设体名须与类名一至）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callback"></param>
    public void HideUI<T>(UnityAction callback = null) where T : BaseUI
    {
        string name = typeof( T ).Name;
        if (!uiDic.ContainsKey(name))
        {
            Debug.LogWarning($"没有找到UI：{name}");
            return;
        }
        if (callback == null) callback = () => 
        {
            GameObject.Destroy(uiDic[name].gameObject);
            uiDic.Remove(name);
            //Debug.Log($"移除{name}");
        };
        else callback += () => 
        {
            GameObject.Destroy(uiDic[name].gameObject);
            uiDic.Remove(name);
            //Debug.Log($"移除{name}");
        };
        uiDic[name].Hide(callback);
    }
}

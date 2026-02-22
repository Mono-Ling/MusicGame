using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    private static EventBus _instance;
    public static EventBus Instance => _instance ?? (_instance = new EventBus());
    private EventBus() 
    {

    }
    private Dictionary<EventType, BaseEventInfo> eventDic = new Dictionary<EventType, BaseEventInfo>();
    public void TriggerEvent(EventType eventType)
    {
        if (!eventDic.ContainsKey(eventType)) return;
        if (eventDic[eventType] is EventInfo)
            (eventDic[eventType] as EventInfo).actions?.Invoke();
        else Debug.LogError($"{eventType}事件参数类型错误");
    }
    public void TriggerEvent<T>(EventType eventType,T p)
    {
        if (!eventDic.ContainsKey(eventType)) return;
        if (eventDic[eventType] is EventInfo<T>)
            (eventDic[eventType] as EventInfo<T>).actions?.Invoke(p);
        else Debug.LogError($"{eventType}事件参数类型错误");
    }
    public void AddListener(EventType eventType,UnityAction callback)
    {
        if (eventDic.ContainsKey(eventType))
        {
            if (eventDic[eventType] is EventInfo)
                (eventDic[eventType] as EventInfo).actions += callback;
            else Debug.LogError($"{eventType}已有含参事件");
        }
        else eventDic.Add(eventType, new EventInfo(callback));
    }
    public void AddListener<T>(EventType eventType, UnityAction<T> callback)
    {
        if (eventDic.ContainsKey(eventType))
        {
            if (eventDic[eventType] is EventInfo<T>)
                (eventDic[eventType] as EventInfo<T>).actions += callback;
            else Debug.LogError($"{eventType}事件参数类型错误");
        }
        else eventDic.Add(eventType, new EventInfo<T>(callback));
    }
    public void RemoveListener(EventType eventType, UnityAction callback)
    {
        if (!eventDic.ContainsKey(eventType)) return;
        if (eventDic[eventType] is EventInfo)
        {
            EventInfo info = eventDic[eventType] as EventInfo;
            info.actions -= callback;
            if(info.actions == null) eventDic.Remove(eventType);
        }
        else Debug.LogError($"{eventType}事件参数类型错误");
    }
    public void RemoveListener<T>(EventType eventType, UnityAction<T> callback)
    {
        if (!eventDic.ContainsKey(eventType)) return;
        if (eventDic[eventType] is EventInfo<T>)
        {
            EventInfo<T> info = eventDic[eventType] as EventInfo<T>;
            info.actions -= callback;
            if (info.actions == null) eventDic.Remove(eventType);
        }
        else Debug.LogError($"{eventType}事件参数类型错误");
    }
    public void Clear()
    {
        eventDic.Clear();
    }
}
public abstract class BaseEventInfo {}
public class EventInfo<T> : BaseEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : BaseEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action) 
    { 
        actions += action;
    }
}

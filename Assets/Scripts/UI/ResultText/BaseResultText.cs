using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseResultText : BaseUI,IPoolItem
{
    public int maxNum = 5;
    public double maxShowTime;
    public double maxHideTime;
    public double showTime;
    public double hideTime;
    public Vector3 maxScale;
    public Vector3 minScale;
    public event UnityAction Reset;
    protected double startTime;
    protected double currentTime;
    protected Vector3 startScale;

    public ExtendType extendType { get; set; } = ExtendType.Reuse;

    protected override void Awake()
    {
        base.Awake();
        EventBus.Instance.AddListener(EventType.EndGame, ForcedRecycle);
    }
    protected virtual void ForcedRecycle()
    {
        hideCallback = null;
        showCallback = null;
        UIManager.Instance.DontBufferHideUI(this,null,false);
        Debug.LogWarning("强制回收");
    }
    public void Init()
    {
        showTime = maxShowTime;
        hideTime = maxHideTime;
        transform.localScale = Vector3.one;
    }
    protected override void Update()
    {
        currentTime = GameTimeManager.Instance.GetGameTime();
        base.Update();
    }
    public override void Show(UnityAction callback = null)
    {
        base.Show(callback);
        startTime = GameTimeManager.Instance.GetGameTime();
        startScale = transform.localScale;
    }
    public override void Hide(UnityAction callback = null, bool isAnimation = true)
    {
        base.Hide(callback,isAnimation);
        startTime = GameTimeManager.Instance.GetGameTime();
        startScale = transform.localScale;
    }
    protected override void ShowAnimation()
    {
        if(currentTime >= startTime + showTime)
        {
            canvasGroup.alpha = 1;
            showCallback?.Invoke();
            showCallback = null;
            return;
        }
        float t = (float)((currentTime - startTime) / showTime);
        t = Mathf.Clamp01(t);
        canvasGroup.alpha = Mathf.Lerp(0, 1, t);
        transform.localScale = Vector3.Lerp(startScale, maxScale, t);
    }
    protected override void HideAnimation()
    {
        if (currentTime >= startTime + hideTime)
        {
            canvasGroup.alpha = 0;
            hideCallback?.Invoke();
            hideCallback = null;
            return;
        }
        float t = (float)((currentTime - startTime) / hideTime);
        t = Mathf.Clamp01(t);
        canvasGroup.alpha = Mathf.Lerp(1, 0, t);
        transform.localScale = Vector3.Lerp(startScale, minScale, t);
    }
    private void OnDestroy()
    {
        hideCallback = null;
        EventBus.Instance.RemoveListener(EventType.EndGame, ForcedRecycle);
    }
    public virtual void Fast(float scale)
    {
        showTime *= scale;
        hideTime *= scale;
    }
    public int GetMaxNum()
    {
        return maxNum;
    }
    private void OnDisable()
    {
        Reset = null;
    }
    public void OnReset()
    {
        Reset?.Invoke();
        Reset = null;
    }
}

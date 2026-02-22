using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseUI : MonoBehaviour
{
    [Header("œ‘“˛ÀŸ∂»")]
    public float speed = 5f;
    protected CanvasGroup canvasGroup;
    protected UnityAction showCallback;
    protected UnityAction hideCallback;
    protected bool isShow;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //canvasGroup = GetComponent<CanvasGroup>();
        InitUI();
    }
    protected abstract void InitUI();
    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShow) ShowAnimation();
        else HideAnimation();
    }
    public virtual void Show(UnityAction callback = null)
    {
        canvasGroup.alpha = 0;
        isShow = true;
        showCallback = callback;
    }
    public virtual void Hide(UnityAction callback = null)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallback = callback;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    protected virtual void ShowAnimation()
    {
        if (canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 1;
            showCallback?.Invoke();
            showCallback = null;
            return;
        }
        canvasGroup.alpha += Time.deltaTime * speed;
    }
    protected virtual void HideAnimation()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            hideCallback?.Invoke();
            hideCallback = null;
            return;
        }
        canvasGroup.alpha -= Time.deltaTime * speed;
    }
}

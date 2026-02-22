using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePanel : BaseUI,IPointerDownHandler,IPointerUpHandler
{
    public event UnityAction<GameObject> InputDown;
    public event UnityAction<GameObject> InputUp;
    public event UnityAction InputPause;
    public Button butPause;
    private Camera mainCamera;
    protected override void InitUI()
    {
        mainCamera = Camera.main;
        if (butPause != null) butPause.onClick.AddListener(() => { InputPause?.Invoke(); });
        else Debug.LogError("ÔÝÍ£°´Å¥Îª¿Õ");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(pointerPosition);
        if (hit != null) InputDown?.Invoke(hit.gameObject);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 pointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(pointerPosition);
        if (hit != null) InputUp?.Invoke(hit.gameObject);
    }
}

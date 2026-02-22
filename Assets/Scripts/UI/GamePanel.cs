using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GamePanel : BaseUI,IPointerDownHandler,IPointerUpHandler
{
    public event UnityAction<GameObject> InputDown;
    public event UnityAction<GameObject> InputUp;
    private Camera mainCamera;
    protected override void InitUI()
    {
        mainCamera = Camera.main;
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

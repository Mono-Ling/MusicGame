using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePanel : BaseUI,IPointerDownHandler,IPointerUpHandler
{
    public event UnityAction<(Track, InputType)> SceenInput;
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
        GetTrackInput(eventData,InputType.Down);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GetTrackInput(eventData,InputType.Up);
    }
    private void GetTrackInput(PointerEventData eventData,InputType inputType)
    {
        Vector2 pointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(pointerPosition);
        if(hit == null) return;
        GameObject obj = hit.gameObject;
        if (obj.CompareTag("Track")) SceenInput?.Invoke((obj.GetComponent<Track>(), inputType));
    }
}

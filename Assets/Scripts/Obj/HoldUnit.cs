using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldUnit : Unit
{
    //public Color edgeStartColor;
    //public Color edgeEndColor;
    //[Range(0f, 1f)]
    //public float edgeThreshold;
    [Header("松开时触发回调的阈值")]
    [Range(0f, 1f)]
    public float clipThreshold = 0.01f;
    private bool isHold;
    private float holdProgress;
    private const string shaderName = "Unlit/HoldUnit";
    protected override void SetScale()
    {
        base.SetScale();
        float far = startPos - hitPos;
        float moveTime = unitHitTime - unitStartTime;
        float step = far / moveTime;
        scaleY = step * unitDuration;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    protected override void InitMaterial()
    {
        shader = Shader.Find(shaderName);
        base.InitMaterial();
        //material.SetColor("_EdgeStartColor",edgeStartColor);
        //material.SetColor("_EdgeEndColor",edgeEndColor);
        //material.SetFloat("_EdgeThreshold", edgeThreshold);
        //material.SetFloat("_EdgePower", 0);
    }
    protected override void Update()
    {
        if (isHold)
        {
            float time = GameManager.Instance.time;
            time = Mathf.InverseLerp(unitHitTime, unitHitTime + unitDuration, time);
            holdProgress = Mathf.SmoothStep(0,1 , time);
            holdProgress = 1 - holdProgress;
            if (holdProgress <= 0) callback?.Invoke();
            material.SetFloat("_HoldProgress", holdProgress);
        }
        else
            base.Update();
    }
    public override void HitUnit(float time,UnityAction callback = null)
    {
        isHold = true;
        //material.SetFloat("_EdgePower", 1);
        hitPos = transform.position.y;
        Debug.Log($"按下{this}");
        this.callback = callback;
    }
    public override void HitUnitEnd(float time)
    {
        isHold = false;
        //material.SetFloat("_EdgePower", 0);
        unitHitTime = time;
        state = UnitState.Miss;
        if(holdProgress < clipThreshold) callback?.Invoke();
        Debug.Log($"松开了{this}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldUnit : Unit
{
    //public Color edgeStartColor;
    //public Color edgeEndColor;
    //[Range(0f, 1f)]
    //public float edgeThreshold;
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
            if (holdProgress <= 0)
            {
                Destroy(gameObject);
                Destroy(material);
            }
            material.SetFloat("_HoldProgress", holdProgress);
            
        }
        else
            base.Update();
    }
    public override void HitUnit(float time)
    {
        isHold = true;
        //material.SetFloat("_EdgePower", 1);
        Debug.Log($"按下{this}");
    }
    public override void HitUnitEnd(float time)
    {
        isHold = false;
        //material.SetFloat("_EdgePower", 0);
        Debug.Log($"松开了{this}");
    }
}

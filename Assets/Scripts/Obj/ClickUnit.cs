using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickUnit : Unit
{
    private const string shaderName = "Unlit/ClickUnit";
    protected override void InitMaterial()
    {
        //shader = Shader.Find(shaderName);
        base.InitMaterial();
    }
    public override void HitUnit(float time = 0, UnityAction callback = null)
    {
        Debug.Log($"点击{this}");
        GameObject eff = Instantiate(Resources.Load<GameObject>("Effect/WaveEff"), transform.position, Quaternion.identity);
        Destroy(eff, 0.5f); // 销毁特效
        callback?.Invoke();
    }
}

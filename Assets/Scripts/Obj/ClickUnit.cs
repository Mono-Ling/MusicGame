using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickUnit : Unit
{
    private const string shaderName = "Unlit/ClickUnit";
    private const string effectPath = "Effect/WaveEff";
    protected override void InitMaterial()
    {
        //shader = Shader.Find(shaderName);
        base.InitMaterial();
    }
    public override void HitUnit(float time = 0, UnityAction callback = null)
    {
        Debug.Log($"点击{this}");
        //GameObject eff = Instantiate(Resources.Load<GameObject>("Effect/WaveEff"), transform.position, Quaternion.identity);
        //Destroy(eff, 0.5f); // 销毁特效
        GameObject effObj = ObjectPool.Instance.GetObject(effectPath);
        effObj.transform.position = transform.position;
        effObj.transform.rotation = Quaternion.identity;
        IPoolItem eff = effObj.GetComponent<IPoolItem>();
        eff?.Init();
        callback?.Invoke();
    }
}

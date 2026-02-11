using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : Unit
{
    private const string shaderName = "Unlit/ClickUnit";
    protected override void Start()
    {
        shader = Shader.Find(shaderName);
        base.Start();
    }
    public override void HitUnit(float time = 0)
    {
        {
            GameObject eff = Instantiate(Resources.Load<GameObject>("Effect/WaveEff"), transform.position, Quaternion.identity);
            Destroy(eff, 0.5f); // Ïú»ÙÌØÐ§
            Destroy(gameObject);
        }
    }
}

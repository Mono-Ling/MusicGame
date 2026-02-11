using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    None,
    Click,
    Hold,
}
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Unit : MonoBehaviour
{
    public float speed = 5f;
    public float scaleX = 1;
    public float scaleY = 1;
    public float unitStartTime = 0;
    public float unitHitTime = 0;
    public float unitDuration = 0;
    public UnitType type = UnitType.None;
    public Shader shader;
    public Color startColor;
    public Color endColor;
    protected float startTime;
    protected float startPos;
    protected float endPos;
    protected Material material;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        startTime = Time.time;
        startPos = transform.position.y;
        endPos = Check.Instance.transform.position.y;
        material = new Material(shader);
        material.hideFlags = HideFlags.HideAndDontSave;
        GetComponent<SpriteRenderer>().material = material;
        material.SetColor("_StartColor", startColor);
        material.SetColor("_EndColor", endColor);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        float t = (Time.time - startTime) / (unitHitTime - unitStartTime);
        if( t >= 1 )
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        else
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startPos,endPos, t), transform.position.z);
    }
    public abstract void HitUnit(float time);
    public virtual void HitUnitEnd(float time)
    {

    }
}

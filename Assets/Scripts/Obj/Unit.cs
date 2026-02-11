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
    public static UnitType GetUnitType(int unitType)
    {
        return (UnitType)unitType;
    }
    public float offsetY = 0.2f;
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
    protected float endTime;
    protected float startPos;
    protected float hitPos;
    protected float endPos;
    protected Material material;
    protected SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        startTime = GameManager.Instance.time;
        startPos = transform.position.y;
        hitPos = Check.Instance.transform.position.y;
        InitMaterial();
        SetScale();
        SetEnd();
    }
    protected virtual void InitMaterial()
    {
        material = new Material(shader);
        material.hideFlags = HideFlags.HideAndDontSave;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = material;
        material.SetColor("_StartColor", startColor);
        material.SetColor("_EndColor", endColor);
    }
    protected virtual void SetScale() 
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    protected virtual void SetEnd()
    {

        float wHeight = 2f * Camera.main.orthographicSize;
        float x1 = startPos - hitPos;
        float x2 = wHeight - x1+ spriteRenderer.bounds.size.y;
        float t1 = unitHitTime - unitStartTime;
        float v = x1 / t1;
        float t2 = x2 / v;
        endTime = unitHitTime + t2;
        endPos = hitPos - x2;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        float t = (GameManager.Instance.time - startTime) / (unitHitTime - unitStartTime);
        float t2 = (GameManager.Instance.time - unitHitTime) / (endTime - unitHitTime);
        if( t >= 1 )
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(hitPos + offsetY, endPos, t2), transform.position.z);
        //transform.Translate(Vector3.down * Time.deltaTime * speed);
        else
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startPos,hitPos, t), transform.position.z);
    }
    public abstract void HitUnit(float time);
    public virtual void HitUnitEnd(float time) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UnitType
{
    None,
    Click,
    Hold,
}
public enum UnitState
{
    Action,
    Miss,
}
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Unit : MonoBehaviour
{
    public static UnitType GetUnitType(int unitType)
    {
        return (UnitType)unitType;
    }
    [Header("触发位置偏移")]
    public float offsetY = 0.2f;
    [Header("缩放")]
    public float scaleX = 1;
    public float scaleY = 1;
    [Header("时间")]
    public float unitStartTime = 0;
    public float unitHitTime = 0;
    public float unitDuration = 0;
    [Header("音符类型")]
    public UnitType type = UnitType.None;
    public UnitState state {  get; private set; }
    [Header("着色器")]
    public Shader shader;
    [Header("颜色设置")]
    public Color startColor;
    public Color endColor;
    protected float startTime;
    protected float endTime;
    protected float startPos;
    protected float hitPos;
    protected float endPos;
    protected Material material;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected float missShaderAlpha = 0.5f;
    protected UnityAction callback;
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
        material.SetFloat("_Alpha", 1f);
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
        float beforeCheckTime = (GameManager.Instance.time - startTime) / (unitHitTime - unitStartTime);
        float afterCheckTime = (GameManager.Instance.time - unitHitTime) / (endTime - unitHitTime);
        if( beforeCheckTime >= 1 )
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(hitPos + offsetY, endPos, afterCheckTime), transform.position.z);
        //transform.Translate(Vector3.down * Time.deltaTime * speed);
        else
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startPos,hitPos, beforeCheckTime), transform.position.z);
        if( afterCheckTime >= 1 )
            callback?.Invoke();
    }
    public abstract void HitUnit(float time,UnityAction callback = null);
    public virtual void HitUnitEnd(float time) { }
    public virtual void UnitMiss(UnityAction callback = null) 
    {
        state = UnitState.Miss;
        material .SetFloat("_Alpha",missShaderAlpha);
        this.callback = callback;
    }
    public virtual void DestoryUnit()
    {
        if(material != null) Destroy(material);
        Destroy(gameObject);
    }
}

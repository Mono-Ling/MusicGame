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
public abstract class Unit : MonoBehaviour,IPoolItem
{
    public static UnitType GetUnitType(int unitType)
    {
        return (UnitType)unitType;
    }
    [Header("对象池最大缓存容量")]
    public int maxNum = 3;
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
    public UnitState state {  get; protected set; }
    public ExtendType extendType { get; set; } = ExtendType.Extend;

    [Header("着色器")]
    public Shader shader;
    [Header("颜色设置")]
    public Color startColor;
    public Color endColor;
    public event UnityAction Reset;
    //protected float startTime;
    protected float endTime;
    protected float startPos;
    protected float hitPos;
    protected float endPos;
    protected Material material;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected float missShaderAlpha = 0.5f;
    protected UnityAction callback;
    protected float moveTime;
    protected UnityAction Action;
    protected Vector2 boundSize;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //startTime = (float)GameManager.Instance.time;
        material = new Material(shader);
        material.hideFlags = HideFlags.HideAndDontSave;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = material;
        boundSize = spriteRenderer.bounds.size;
        OnAction();
        Action = OnAction;
    }
    protected virtual void OnAction()
    {
        callback = null;
        state = UnitState.Action;
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveTime = GameManager.Instance.moveTime;
        startPos = transform.position.y;
        hitPos = Check.Instance.transform.position.y;
        InitMaterial();
        SetScale();
        SetEnd();
    }
    public void Init()
    {
        Action?.Invoke();
    }
    protected virtual void InitMaterial()
    {
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
        //float x2 = wHeight - x1+ spriteRenderer.bounds.size.y;
        float distanceToEnd = wHeight + spriteRenderer.bounds.size.y;
        float t1 = moveTime;
        float v = x1 / t1;
        float t2 = distanceToEnd / v;
        endTime = unitHitTime + t2;
        endPos = hitPos - distanceToEnd;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        moveTime = GameManager.Instance.moveTime;
        float beforeCheckTime = ((float)GameManager.Instance.currentTime - unitStartTime) / (unitHitTime - unitStartTime);
        float afterCheckTime = ((float)GameManager.Instance.currentTime - unitHitTime) / (endTime - unitHitTime);
        if( beforeCheckTime >= 1 )
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(hitPos, endPos, afterCheckTime), transform.position.z);
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
        callback += DestoryUnit;
        this.callback = callback;
    }
    public virtual void DestoryUnit()
    {
        //if(material != null) Destroy(material);
        //Destroy(gameObject);
        ObjectPool.Instance.PutObject(gameObject);
    }
    private void OnDestroy()
    {
        if(material != null) Destroy(material);
        OnReset();
    }
    public void OnReset()
    {
        Reset?.Invoke();
        Reset = null;
    }
    public int GetMaxNum() { return maxNum; }
}

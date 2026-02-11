using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5f;
    public float scaleX = 1;
    public float unitStartTime = 0;
    public float unitEndTime = 0;
    private CapsuleCollider2D capsuleCollider;
    private float startTime;
    private float startPos;
    private float endPos;
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        if(capsuleCollider == null )
        {
            Debug.LogWarning("预设体缺少CapsuleCollider2D组件");
            capsuleCollider = gameObject.AddComponent<CapsuleCollider2D>();
        }
        capsuleCollider.enabled = true;
        capsuleCollider.size = new Vector2(scaleX + 0.5f, 1);
        transform.localScale = new Vector3(scaleX, 1, 1);
        startTime = Time.time;
        startPos = transform.position.y;
        endPos = Check.Instance.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time - startTime) / (unitEndTime - unitStartTime);
        if( t >= 1 )
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        else
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startPos,endPos, t), transform.position.z);
    }
}

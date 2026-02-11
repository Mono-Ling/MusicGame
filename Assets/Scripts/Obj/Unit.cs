using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5f;
    public float scaleX = 1;
    public float unitStartTime = 0;
    public float unitEndTime = 0;
    private float startTime;
    private float startPos;
    private float endPos;
    // Start is called before the first frame update
    void Start()
    {
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
    public void HitUnit()
    {
        GameObject eff = Instantiate(Resources.Load<GameObject>("Effect/WaveEff"), transform.position, Quaternion.identity);
        Destroy(eff, 0.5f); // Ïú»ÙÌØÐ§
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    private static Check _instance;
    public static Check Instance => _instance;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning($"单例{_instance}重复注册");
            return;
        }
        _instance = this;
    }
    public float height;
    [Header("轨道基线")]
    public Material material;
    public Color color;
    public float lineWidth;
    private const int lineCount = 5;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        transform.localScale = new Vector3(scale.x * 2, transform.localScale.y, transform.localScale.z);

        float height = Camera.main.orthographicSize * 2f;
        Vector3 pos = new Vector3(-height * Camera.main.aspect/2,- height/2,0);
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        Vector3 step = Camera.main.ScreenToWorldPoint(new Vector3(screenStep.x, screenStep.y, 0));
        step = new Vector3(step.x/2, -step.y * 2,0);
        for (int i = 0; i < lineCount; i++)
        {
            GameObject lineObj = new GameObject();
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.startColor = color;
            line.endColor = color;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.material = material;
            line.SetPositions(new Vector3[]{pos - new Vector3(step.x * (i + 1),0,0) ,
                                            pos - new Vector3(step.x * (i + 1),step.y,0) });
            lineObj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("InvalidMusicUnit"))
    //    {
    //        collision.gameObject.tag = "MusicUnit";
    //        Debug.Log("进入检测区域");
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("MusicUnit"))
    //    {
    //        collision.gameObject.tag = "InvalidMusicUnit";
    //        Debug.Log("离开检测区域");
    //    }
    //}
}

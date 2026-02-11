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
    private BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        transform.localScale = new Vector3(scale.x * 2, transform.localScale.y, transform.localScale.z);
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            Debug.LogWarning("缺少碰撞盒");
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCollider2D.size = new Vector2(1, height);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InvalidMusicUnit"))
        {
            collision.gameObject.tag = "MusicUnit";
            Debug.Log("进入检测区域");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MusicUnit"))
        {
            collision.gameObject.tag = "InvalidMusicUnit";
            Debug.Log("离开检测区域");
        }
    }
}

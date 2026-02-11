using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("场景中已经存在InputManager实例，新的实例将被销毁");
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public Material hitMaterial; // 用于点击音符时的材质
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("MusicUnit"))
            {
                Debug.Log("点击了音符");
                ScoreManager.Instance.AddScore(10); // 增加分数
                Unit unit = hit.gameObject.GetComponent<Unit>();
                Destroy(hit.gameObject);
                GameObject eff = Instantiate(Resources.Load<GameObject>("Effect/WaveEff"), hit.transform.position, Quaternion.identity);
                Destroy(eff, 0.5f); // 销毁特效
            }
        }
    }
}

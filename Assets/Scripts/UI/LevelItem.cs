using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItem : MonoBehaviour
{
    public float maxScaleX;
    public float maxScaleY;
    public float minScaleX;
    public float minScaleY;
    public LevelData levelData;
    private float scaleX;
    private float scaleY;
    private float halfHeight;
    private SelectLevelPanel selectPanel;
    // Start is called before the first frame update
    void Start()
    {
        selectPanel = UIManager.Instance.GetUI<SelectLevelPanel>();
        //halfHeight = selectPanel.height/2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(halfHeight - selectPanel.half) > 0.01f)
        {
            halfHeight = selectPanel.half;
        }
        float far = halfHeight - Mathf.Abs(transform.position.y - halfHeight);
        float time = far / halfHeight;
        float timeX = Mathf.Clamp(time, minScaleX/maxScaleX, 1);
        float timeY = Mathf.Clamp(time, minScaleY/maxScaleY, 1);
        scaleX = maxScaleX * timeX;
        scaleY = maxScaleY * timeY;
        transform.localScale = new Vector3(scaleX, scaleY,1);
    }
}

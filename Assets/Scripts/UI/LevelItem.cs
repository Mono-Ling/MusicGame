using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public float maxScaleX;
    public float maxScaleY;
    public float minScaleX;
    public float minScaleY;
    public LevelData levelData;
    public TextMeshProUGUI tmpText;
    public Text text;
    public float scaleX;
    public float scaleY;
    private float halfHeight;
    private float textSize;
    private SelectLevelPanel selectPanel;
    // Start is called before the first frame update
    void Start()
    {
        selectPanel = UIManager.Instance.GetUI<SelectLevelPanel>();
        //halfHeight = selectPanel.height/2;
        textSize = tmpText.fontSize;
        //scaleX = 300;//Screen.width / 4;
        //scaleY = 150;//Screen.height / 4;
        if(levelData == null )
        {
            Debug.LogError("LevelDataÎª¿Õ");
            return;
        }
        if(text != null ) SetName(levelData.name);
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
        //scaleX = maxScaleX * timeX;
        //scaleY = maxScaleY * timeY;
        //transform.localScale = new Vector3(scaleX, scaleY,1);
        RectTransform rect = transform as RectTransform;
        rect.sizeDelta = new Vector2(scaleX * timeX, scaleY * timeY);
        SetTextSize(textSize * timeX);
    }
    private void SetName(string name)
    {
        tmpText.text = name;
    }
    private void SetTextSize(float size)
    {
        tmpText.fontSize = size;
    }
}

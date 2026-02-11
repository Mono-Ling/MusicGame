using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private static UnitManager instance;
    public static UnitManager Instance => instance;
    private void Awake()
    {
        if(instance!=null&&instance!=this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    public List<Track> tracks = new List<Track>();
    public float window = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float time = GameManager.Instance.time;
        UpdateActionUnitList(time);
        CheckInput(time);
    }
    /// <summary>
    /// 更新各轨道上激活音符列表
    /// </summary>
    private void UpdateActionUnitList(float time)
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            if (tracks[i].actionUnits.Count == 0)
                continue;
            Unit unit = tracks[i].actionUnits[0];
            if (unit == null || time - unit.unitEndTime > window)
            {
                tracks[i].actionUnits.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// 检测输入
    /// </summary>
    /// <param name="time"></param>
    private void CheckInput(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("Track"))
            {
                Debug.Log("点击了轨道");
                int hitTrackIndex = hit.gameObject.GetComponent<Track>().id - 1;
                Unit unit = tracks[hitTrackIndex].ComparInputUnit(time,window);
                if (unit != null) unit.HitUnit();
            }
        }
    }
    public void CreatUnit(UnitData unitData)
    {
        GameObject unitPrefab = Resources.Load<GameObject>($"MusicGameUnit/{unitData.unitType}");
        GameObject unitObj = Instantiate(unitPrefab);
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        //unitObj.transform.position = new Vector3(unitData.trackId * 2 - 4, 6, 0);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.scaleX = screenStep.x / 100f;
        unit.unitEndTime = unitData.endTime;
        unit.unitStartTime = unitData.startTime;
        unitObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenStep.x * (unitData.trackId + 0.5f), screenStep.y, 0));
        unitObj.transform.position = new Vector3(unitObj.transform.position.x, unitObj.transform.position.y + 0.5f, 0);
        tracks[unitData.trackId-1].actionUnits.Add(unit);
    }
}
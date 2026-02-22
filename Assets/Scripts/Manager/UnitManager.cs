using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private Camera mainCamera;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        InputManager.Instance.TrackDown_1 += () => { InputDown(tracks[0], time); };
        InputManager.Instance.TrackDown_2 += () => { InputDown(tracks[1], time); };
        InputManager.Instance.TrackDown_3 += () => { InputDown(tracks[2], time); };
        InputManager.Instance.TrackDown_4 += () => { InputDown(tracks[3], time); };

        InputManager.Instance.TrackUp_1 += () => { InputUp(tracks[0], time); };
        InputManager.Instance.TrackUp_2 += () => { InputUp(tracks[1], time); };
        InputManager.Instance.TrackUp_3 += () => { InputUp(tracks[2], time); };
        InputManager.Instance.TrackUp_4 += () => { InputUp(tracks[3], time); };

        InputManager.Instance.ScreenInputTrackDown += (track) => { InputDown(track, time); };
        InputManager.Instance.ScreenInputTrackUp += (track) => { InputUp(track, time); };
    }

    // Update is called once per frame
    void Update()
    {
        time = (float)GameManager.Instance.currentTime;
        UpdateActionUnitList(time);
        //CheckInput(time);
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
            if (unit == null || time - unit.unitHitTime > window)
            {
                unit.UnitMiss(() => { unit.DestoryUnit(); });
                tracks[i].actionUnits.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// 检测输入
    /// </summary>
    /// <param name="time"></param>
    /*private void CheckInput(float time)
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
        //    if (hit != null && hit.CompareTag("Track"))
        //    {
        //        Track track = hit.gameObject.GetComponent<Track>();
        //        Debug.Log($"点击了轨道{track.id}");
        //        Unit unit = track.ComparInputUnit(time,window);
        //        if (unit != null) unit.HitUnit(time, () => { unit.DestoryUnit(); });
        //        if(unit != null&&unit.type == UnitType.Hold) track.holdingUnit = unit;
        //    }
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
        //    if (hit != null && hit.CompareTag("Track"))
        //    {
        //        Track track = hit.gameObject.GetComponent<Track>();
        //        if(track.holdingUnit != null)
        //        {
        //            track.holdingUnit.HitUnitEnd(time);
        //            track.holdingUnit = null;
        //        }
        //    }
        //}
    }*/
    private void InputDown(Track track,float time)
    {
        Unit unit = track.ComparInputUnit(time, window);
        if (unit != null) unit.HitUnit(time, () => { unit.DestoryUnit(); });
        if (unit != null && unit.type == UnitType.Hold) track.holdingUnit = unit;
    }
    private void InputUp(Track track,float time)
    {
        if (track.holdingUnit != null)
        {
            track.holdingUnit.HitUnitEnd(time);
            track.holdingUnit = null;
        }
    }
    public void CreateUnit(UnitData unitData,float moveTime = 2)
    {
        GameObject unitPrefab = Resources.Load<GameObject>($"MusicGameUnit/{unitData.unitType}");
        GameObject unitObj = Instantiate(unitPrefab);
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        //unitObj.transform.position = new Vector3(unitData.trackId * 2 - 4, 6, 0);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.scaleX = mainCamera.orthographicSize * 2 * mainCamera.aspect/6f; //screenStep.x / 100f;
        unit.unitHitTime = unitData.hitTime;
        unit.unitStartTime = unitData.startTime;
        unit.unitDuration = unitData.duration;
        unit.type = Unit.GetUnitType(unitData.unitType);
        unitObj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(screenStep.x * (unitData.trackId + 0.5f), screenStep.y, 0));
        unitObj.transform.position = new Vector3(unitObj.transform.position.x, unitObj.transform.position.y, 0);
        tracks[unitData.trackId-1].actionUnits.Add(unit);
    }
}
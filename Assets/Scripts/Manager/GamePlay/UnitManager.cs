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
    public float maxWindow = 0.15f;
    public float window = 1;
    private Camera mainCamera;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        EventBus.Instance.AddListener(EventType.Track_1_Down,Track_1_Down);
        EventBus.Instance.AddListener(EventType.Track_2_Down, Track_2_Down);
        EventBus.Instance.AddListener(EventType.Track_3_Down, Track_3_Down);
        EventBus.Instance.AddListener(EventType.Track_4_Down, Track_4_Down);

        EventBus.Instance.AddListener(EventType.Track_1_Up, Track_1_Up);
        EventBus.Instance.AddListener(EventType.Track_2_Up, Track_2_Up);
        EventBus.Instance.AddListener(EventType.Track_3_Up, Track_3_Up);
        EventBus.Instance.AddListener(EventType.Track_4_Up, Track_4_Up);

        EventBus.Instance.AddListener<Track>(EventType.ScreenInputTrackDown, ScreenInputDown);
        EventBus.Instance.AddListener<Track>(EventType.ScreenInputTrackUp, ScreenInputUp);
    }

    // Update is called once per frame
    void Update()
    {
        time = (float)GameManager.Instance.currentTime;
        window = maxWindow * GameManager.Instance.GetWindowScale();
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
    private void Track_1_Down(){ InputDown(tracks[0], time);}
    private void Track_2_Down() { InputDown(tracks[1], time); }
    private void Track_3_Down() { InputDown(tracks[2], time); }
    private void Track_4_Down() { InputDown(tracks[3], time); }

    private void Track_1_Up() { InputUp(tracks[0], time); }
    private void Track_2_Up() { InputUp(tracks[1], time); }
    private void Track_3_Up() { InputUp(tracks[2], time); }
    private void Track_4_Up() { InputUp(tracks[3], time); }

    private void ScreenInputDown(Track track) { InputDown(track, time); }
    private void ScreenInputUp(Track track) { InputUp(track, time); }

    private void InputDown(Track track,float time)
    {
        Unit unit = track.ComparInputUnit(time, window);
        if (unit != null) unit.HitUnit(time, () => { unit.DestoryUnit(); });
        if (unit != null && unit.type == UnitType.Hold)
        {
            track.holdingUnit = unit;
            unit.Reset += () =>
            {
                track.holdingUnit = null;
            };
        }
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
        //GameObject unitPrefab = Resources.Load<GameObject>($"MusicGameUnit/{unitData.unitType}");
        //GameObject unitObj = Instantiate(unitPrefab);
        GameObject unitObj = ObjectPool.Instance.GetObject($"MusicGameUnit/{unitData.unitType}");
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        //unitObj.transform.position = new Vector3(unitData.trackId * 2 - 4, 6, 0);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.scaleX = mainCamera.orthographicSize * 2 * mainCamera.aspect/6f; //screenStep.x / 100f;
        unit.unitHitTime = unitData.hitTime;
        unit.unitStartTime = unitData.startTime;
        unit.unitDuration = unitData.duration;
        unit.type = Unit.GetUnitType(unitData.unitType);
        unitObj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(screenStep.x * (unitData.trackId + 0.5f), screenStep.y, 0));
        unitObj.transform.position = new Vector3(unitObj.transform.position.x, unitObj.transform.position.y + 0.5f, 0);
        tracks[unitData.trackId-1].actionUnits.Add(unit);
        unit.Init();
    }
    private void OnDestroy()
    {
        EventBus.Instance.RemoveListener(EventType.Track_1_Down, Track_1_Down);
        EventBus.Instance.RemoveListener(EventType.Track_2_Down, Track_2_Down);
        EventBus.Instance.RemoveListener(EventType.Track_3_Down, Track_3_Down);
        EventBus.Instance.RemoveListener(EventType.Track_4_Down, Track_4_Down);

        EventBus.Instance.RemoveListener(EventType.Track_1_Up, Track_1_Up);
        EventBus.Instance.RemoveListener(EventType.Track_2_Up, Track_2_Up);
        EventBus.Instance.RemoveListener(EventType.Track_3_Up, Track_3_Up);
        EventBus.Instance.RemoveListener(EventType.Track_4_Up, Track_4_Up);

        EventBus.Instance.RemoveListener<Track>(EventType.ScreenInputTrackDown, ScreenInputDown);
        EventBus.Instance.RemoveListener<Track>(EventType.ScreenInputTrackUp, ScreenInputUp);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        EventBus.Instance.AddListener<InputType>(EventType.Track_1,Track_1);
        EventBus.Instance.AddListener<InputType>(EventType.Track_2, Track_2);
        EventBus.Instance.AddListener<InputType>(EventType.Track_3, Track_3);
        EventBus.Instance.AddListener<InputType>(EventType.Track_4, Track_4);

        EventBus.Instance.AddListener<(Track,InputType)>(EventType.ScreenInput,ScreenInput);
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
            int currentIndex = tracks[i].currentUnitIndex;
            if(currentIndex >= tracks[i].actionUnits.Count) continue;
            Unit unit = tracks[i].actionUnits[currentIndex];
            if (unit == null || time - unit.unitHitTime > window)
            {
                unit.UnitMiss(/*() => { unit.DestoryUnit(); }*/);//miss回调函数中已添加销毁对象
                //tracks[i].actionUnits.RemoveAt(0);
                tracks[i].currentUnitIndex++;

                DisplayResultManager.Instance.GetMissResult();
            }
        }
    }

    private void Track_1(InputType inputType)
    {
        KeyTrackInput(tracks[0], inputType);
    }
    private void Track_2(InputType inputType)
    {
        KeyTrackInput(tracks[1], inputType);
    }
    private void Track_3(InputType inputType)
    {
        KeyTrackInput(tracks[2], inputType);
    }
    private void Track_4(InputType inputType)
    {
        KeyTrackInput(tracks[3],inputType);
    }
    private void KeyTrackInput(Track track, InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Down:
                InputDown(track, time);
                break;
            case InputType.Up:
                InputUp(track, time);
                break;
            default:
                break;
        }
    }

    private void ScreenInput((Track track, InputType inputType) input)
    {
        switch(input.inputType)
        {
            case InputType.Down:
                InputDown(input.track, time);
                break;
            case InputType.Up:
                InputUp(input.track, time);
                break;
        }
    }

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
        EventBus.Instance.RemoveListener<InputType>(EventType.Track_1, Track_1);
        EventBus.Instance.RemoveListener<InputType>(EventType.Track_2, Track_2);
        EventBus.Instance.RemoveListener<InputType>(EventType.Track_3, Track_3);
        EventBus.Instance.RemoveListener<InputType>(EventType.Track_4, Track_4);

        EventBus.Instance.RemoveListener<(Track, InputType)>(EventType.ScreenInput, ScreenInput);
    }
}
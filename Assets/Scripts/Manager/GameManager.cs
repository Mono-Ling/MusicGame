using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning($"单例{_instance}重复注册");
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public Queue<UnitData> musicGameUnitQueue = new Queue<UnitData>();
    private List<UnitData> musicGameUnitList = DataManager.Instance.unitDataList.unitList;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        if(musicGameUnitList == null || musicGameUnitList.Count == 0)
        {
            Debug.LogError("音游单位数据列表为空");
            return;
        }
        foreach (var unitData in musicGameUnitList)
        {
            musicGameUnitQueue.Enqueue(unitData);
        }
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UpdateMusicGameUnit());
            source.enabled = true;
        }
    }
    IEnumerator UpdateMusicGameUnit()
    {
        UnitData firstUnitData = musicGameUnitQueue.Peek();
        yield return new WaitForSeconds(firstUnitData.startTime);
        while (musicGameUnitQueue.Count > 0)
        {
            UnitData unitData = musicGameUnitQueue.Dequeue();
            CreatUnit(unitData);
            if (musicGameUnitQueue.Count == 0) break;
            float timeToNextUnit = musicGameUnitQueue.Peek().startTime - unitData.startTime;
            yield return new WaitForSeconds(timeToNextUnit);
        }
        Debug.Log("协程结束");
    }
    private void CreatUnit(UnitData unitData)
    {
        GameObject unitPrefab = Resources.Load<GameObject>($"MusicGameUnit/{unitData.unitType}");
        GameObject unitObj = Instantiate(unitPrefab);
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        //unitObj.transform.position = new Vector3(unitData.trackId * 2 - 4, 6, 0);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.scaleX = screenStep.x/100f;
        unit.unitEndTime = unitData.endTime;
        unit.unitStartTime = unitData.startTime;
        unitObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenStep.x *(unitData.trackId+0.5f), screenStep.y, 0));
        unitObj.transform.position = new Vector3(unitObj.transform.position.x, unitObj.transform.position.y+0.5f, 0);
    }
}

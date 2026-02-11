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
    // Start is called before the first frame update
    void Start()
    {
        if(musicGameUnitList == null || musicGameUnitList.Count == 0)
        {
            Debug.LogError("音乐游戏单位数据列表为空");
            return;
        }
        foreach (var unitData in musicGameUnitList)
        {
            musicGameUnitQueue.Enqueue(unitData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UpdateMusicGameUnit());
        }
    }
    IEnumerator UpdateMusicGameUnit()
    {
        while(musicGameUnitQueue.Count > 0)
        {
            UnitData unitData = musicGameUnitQueue.Dequeue();
            CreatUnit(unitData);
            if (musicGameUnitQueue.Count == 0) break;
            float timeToNextUnit = musicGameUnitQueue.Peek().time - unitData.time;
            yield return new WaitForSeconds(timeToNextUnit);
        }
        Debug.Log("协程结束");
    }
    private void CreatUnit(UnitData unitData)
    {
        GameObject unitPrefab = Resources.Load<GameObject>($"MusicGameUnit/{unitData.unitType}");
        GameObject unitObj = Instantiate(unitPrefab);
        unitObj.transform.position = new Vector3(unitData.trackId * 2 - 4, 6, 0);
    }
}

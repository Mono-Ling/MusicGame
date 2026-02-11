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
        //DontDestroyOnLoad(gameObject);
    }
    public Queue<UnitData> unitDataQueue = new Queue<UnitData>();
    public float time {  get; private set; }
    private bool musicPlaying = false;
    private List<UnitData> unitDataList = DataManager.Instance.unitDataList.unitList;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        if(unitDataList == null || unitDataList.Count == 0)
        {
            Debug.LogError("音游单位数据列表为空");
            return;
        }
        foreach (var unitData in unitDataList)
        {
            unitDataQueue.Enqueue(unitData);
        }
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            Debug.LogWarning($"{source}为空，已更新组件");
            source = gameObject.AddComponent<AudioSource>();
        }
        source.loop = false;
        source.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UpdateMusicGameUnit());
            musicPlaying = true;
            source.enabled = true;
        }
        if(musicPlaying)
        {
            time += Time.deltaTime;
        }
    }
    IEnumerator UpdateMusicGameUnit()
    {
        UnitData firstUnitData = unitDataQueue.Peek();
        yield return new WaitForSeconds(firstUnitData.startTime);
        while (unitDataQueue.Count > 0)
        {
            UnitData unitData = unitDataQueue.Dequeue();
            UnitManager.Instance.CreateUnit(unitData);
            if (unitDataQueue.Count == 0) break;
            float timeToNextUnit = unitDataQueue.Peek().startTime - unitData.startTime;
            yield return new WaitForSeconds(timeToNextUnit);
        }
        Debug.Log("协程结束");
    }
}

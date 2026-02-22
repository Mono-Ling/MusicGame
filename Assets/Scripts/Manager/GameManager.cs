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
    public float moveTime = 2;
    public double time {  get; private set; }
    public int levelIndex;
    private bool musicPlaying = false;
    private List<UnitData> unitDataList;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        unitDataList = SelectLevelManager.Instance.GetLevelUnitDatas();
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
        source.clip = SelectLevelManager.Instance.GetLevelMusicClip();
        source.loop = false;
        source.Stop();
        GameTimeManager.Instance.PauseGame(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UpdateMusicGameUnit());
            source.Play();
            GameTimeManager.Instance.PauseGame(false);
            musicPlaying = true;
        }
        //time = (float)GameTimeManager.Instance.GetGameTime();
        if (musicPlaying)
            time = GameTimeManager.Instance.GetGameTime();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameTimeManager.Instance.PauseGame(musicPlaying);
            musicPlaying = !musicPlaying;
            print("暂停");
        }
    }
    IEnumerator UpdateMusicGameUnit()
    {
        UnitData firstUnitData = unitDataQueue.Peek();
        firstUnitData.SetStartTime(moveTime);
        yield return new WaitForDSPTime(firstUnitData.startTime);
        while (unitDataQueue.Count > 0)
        {
            UnitData unitData = unitDataQueue.Dequeue();
            //unitData.SetStartTime(moveTime);
            UnitManager.Instance.CreateUnit(unitData);
            if (unitDataQueue.Count == 0) break;
            UnitData nextUnit = unitDataQueue.Peek();
            nextUnit.SetStartTime(moveTime);
            float timeToNextUnit = nextUnit.startTime - (float)time;
            yield return new WaitForDSPTime(timeToNextUnit);
        }
        Debug.Log("协程结束");
    }
}

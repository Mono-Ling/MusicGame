using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public double currentTime {  get; private set; }
    public int levelIndex;
    private bool isPlaying = false;
    private List<UnitData> unitDataList;
    private AudioSource source;
    private float time;
    //private UnityAction StartGame;
    // Start is called before the first frame update
    void Start()
    {
        unitDataList = SelectLevelManager.Instance.GetLevelUnitDatas();
        time = SelectLevelManager.Instance.GetTime();
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
        GameTimeManager.Instance.SetStartTime();
        GameTimeManager.Instance.PauseGame(true);
        StartCoroutine(GameProgress());
        UIManager.Instance.ShowUI<InitGame>();
        InputManager.Instance.StartGame += ()=> UIManager.Instance.HideUI<InitGame>(StartGame);
        InputManager.Instance.PauseGame += () => PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = GameTimeManager.Instance.GetGameTime();
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
            float timeToNextUnit = nextUnit.startTime - (float)currentTime;
            yield return new WaitForDSPTime(timeToNextUnit);
        }
        Debug.Log("协程结束");
    }
    IEnumerator GameProgress()
    {
        while(currentTime<time)
        {
            yield return null;
        }
        UIManager.Instance.ShowUI<EndGame>();
    }
    private void StartGame()
    {
        StartCoroutine(UpdateMusicGameUnit());
        source.Play();
        GameTimeManager.Instance.PauseGame(false);
        isPlaying = true;
    }
    private void PauseGame()
    {
        GameTimeManager.Instance.PauseGame(isPlaying);
        isPlaying = !isPlaying;
        print("暂停");
    }
}

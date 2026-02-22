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
    private AudioSource source { get; set; }
    private float time;
    private List<KeyframeData> keyframeDatas;
    private Queue<KeyframeData> keyframeDataQueue = new Queue<KeyframeData>();
    private float maxMoveTime = 2;
    private AudioClip audioClip;
    //private UnityAction StartGame;
    // Start is called before the first frame update
    void Start()
    {
        unitDataList = SelectLevelManager.Instance.GetLevelUnitDatas();
        keyframeDatas = SelectLevelManager.Instance.GetUnitMoveTimeKeyframeDatas();
        maxMoveTime = SelectLevelManager.Instance.GetUnitMaxMoveTime();
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
        //source = GetComponent<AudioSource>();
        //if (source == null)
        //{
        //    Debug.LogWarning($"{source}为空，已更新组件");
        //    source = gameObject.AddComponent<AudioSource>();
        //}
        audioClip = SelectLevelManager.Instance.GetLevelMusicClip();
        AudioManager.Instance.SetMusic(audioClip);
        //source.loop = false;
        //source.Stop();
        GameTimeManager.Instance.SetStartTime();
        GameTimeManager.Instance.PauseGame(true);
        StartCoroutine(GameProgress());
        UIManager.Instance.ShowUI<InitGame>();
        //InputManager.Instance.StartGame += ()=> UIManager.Instance.HideUI<InitGame>(StartGame);
        //InputManager.Instance.PauseGame += () => PauseGame();

        EventBus.Instance.AddListener(EventType.StartGame, StartGame);
        EventBus.Instance.AddListener(EventType.PauseGame, PauseGame);

        InputManager.Instance.StartInput();

        if (keyframeDatas == null || keyframeDatas.Count == 0)
        {
            Debug.Log("音符移动时间配置文件为空");
            return;
        }
        foreach (var keyframeData in keyframeDatas)
        {
            keyframeDataQueue.Enqueue(keyframeData);
        }
        //if (keyframeDataQueue != null && keyframeDataQueue.Count > 0)
        //{
        //    //InputManager.Instance.StartGame += () => { StartCoroutine(UpdateMoveTime()); };
        //}
        //else Debug.LogWarning($"{this}音符移动速度插值协程初始化异常");
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
        UIManager.Instance.HideUI<InitGame>(StartGamePlay);
    }
    private void StartGamePlay()
    {
        StartCoroutine(UpdateMusicGameUnit());
        //source.Play();
        AudioManager.Instance.PlayMusic();
        GameTimeManager.Instance.PauseGame(false);
        isPlaying = true;
        if (keyframeDataQueue != null && keyframeDataQueue.Count > 0)
            StartCoroutine(UpdateMoveTime());
        else Debug.LogWarning($"{this}音符移动速度插值协程初始化异常");
        EventBus.Instance.RemoveListener(EventType.StartGame,StartGame);
    }
    private void PauseGame()
    {
        GameTimeManager.Instance.PauseGame(isPlaying);
        isPlaying = !isPlaying;
        print("暂停");
    }
    IEnumerator UpdateMoveTime()
    {
        float currentValue = 0f;
        //Debug.Log($"{this}协程开启");
        while (keyframeDataQueue.Count > 0)
        {
            float startTime = (float)GameManager.Instance.currentTime;
            float endTime = keyframeDataQueue.Peek().time;
            float endValue = keyframeDataQueue.Peek().value;
            float startValue = currentValue;
            if (Mathf.Approximately(endTime, startTime))
            {
                currentValue = endValue;
                //bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, maxSpeed, currentValue * maxSpeed);
                moveTime = maxMoveTime - maxMoveTime * currentValue;
                keyframeDataQueue.Dequeue();
                continue;
            }
            while (true)
            {
                float currentTime = (float)GameManager.Instance.currentTime;
                if (currentTime >= endTime)
                {
                    currentValue = endValue;
                    //float clampedValue = Mathf.Clamp(currentValue, 0, bloomMaxValue);
                    //bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, maxSpeed, currentValue * maxSpeed);
                    moveTime = maxMoveTime - maxMoveTime * currentValue;
                    break;
                }
                float t = (currentTime - startTime) / (endTime - startTime);
                t = Mathf.Clamp01(t);
                currentValue = Mathf.Lerp(startValue, endValue, t);
                //bloom.LThreshold = 1 - Mathf.SmoothStep(currentValue, maxSpeed, currentValue * maxSpeed);
                moveTime = maxMoveTime - maxMoveTime * currentValue;
                yield return null;
            }
            keyframeDataQueue.Dequeue();
        }
        Debug.Log($"{this}协程结束");
    }
    public float GetWindowScale()
    {
        return Mathf.Clamp(moveTime/maxMoveTime, 0.2f, 1);
    }
    private void OnDestroy()
    {
        EventBus.Instance.RemoveListener(EventType.PauseGame,PauseGame);
        InputManager.Instance.StopInput();
    }
}

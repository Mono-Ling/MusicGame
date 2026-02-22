using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject($"{typeof(InputManager)}");
                _instance = obj.AddComponent<InputManager>();
                DontDestroyOnLoad( obj );
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("场景中已经存在InputManager实例，新的实例将被销毁");
            return;
        }
    }
    private UnityAction StartGame;
    //public event UnityAction PauseGame;
    private Dictionary<EventType, KeyCode> keyInputDic;
    private GamePanel gamePanel;
    private bool isRunning = false;
    private bool isCheckInput = false;
    private UnityAction<KeyCode> checkCallback;
    // Start is called before the first frame update
    void Start()
    {
        keyInputDic = SettingManager.Instance.keySettingDic;
        if(keyInputDic != null && keyInputDic.Count > 0) return;
        keyInputDic = new Dictionary<EventType, KeyCode>();
        SetKeyInput(EventType.Track_1, KeyCode.A);
        SetKeyInput(EventType.Track_2, KeyCode.S);
        SetKeyInput(EventType.Track_3, KeyCode.K);
        SetKeyInput(EventType.Track_4, KeyCode.L);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckInput) GetKeyInput(checkCallback);
        if (!isRunning) return;
        if (Input.anyKeyDown && StartGame != null)
        {
            //StartGame += () => { StartGame = null; };
            //StartGame?.Invoke();
            EventBus.Instance.TriggerEvent(EventType.StartGame);
        }
        KeyInput();
    }
    public void SetKeyInput(EventType eventType,KeyCode key)
    {
        if (keyInputDic.ContainsKey(eventType)) keyInputDic[eventType] = key;
        else keyInputDic.Add(eventType, key);
    }
    public void RemoveKeyInput(EventType eventType)
    {
        if(keyInputDic.ContainsKey(eventType)) keyInputDic.Remove(eventType);
    }
    public void SetKeyInputDic(Dictionary<EventType,KeyCode> keyInputDic)
    {
        if (keyInputDic == null)
        {
            Debug.LogError("按键映射字典不能为空");
            return;
        }
        this.keyInputDic = keyInputDic;
    }
    public void ClearKeyInputDic()
    {
        keyInputDic.Clear();
    }
    public void StartInput()
    {
        UIManager.Instance.ShowUI<GamePanel>();
        gamePanel = UIManager.Instance.GetUI<GamePanel>();
        gamePanel.InputDown += ScreenInputDown;
        gamePanel.InputUp += ScreenInputUp;
        gamePanel.InputPause += () =>
        {
            //PauseGame?.Invoke(); 
            EventBus.Instance.TriggerEvent(EventType.PauseGame);
        };
        StartGame += () => { EventBus.Instance.TriggerEvent(EventType.StartGame); };
        isRunning = true;
    }
    public void StopInput()
    {
        UIManager.Instance.HideUI<GamePanel>();
        StartGame = null;
        isRunning = false;
    }
    public void StartCheck(UnityAction<KeyCode> action)
    {
        checkCallback = action;
        StartCoroutine(CheckDelay());
    }
    private IEnumerator CheckDelay()
    {
        yield return null;
        isCheckInput = true;
    }
    public void StopCheck()
    {
        isCheckInput = false;
        checkCallback = null;
    }
    private void GetKeyInput(UnityAction<KeyCode> callback)
    {
        Array keycods = Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode key in keycods)
        {
            if (Input.GetKeyDown(key))
            {
                callback?.Invoke(key);
                break;
            }
        }
    }
    private void KeyInput()
    {
        foreach(var item in keyInputDic)
        {
            KeyInputUnit(item.Key, item.Value);
        }
    }
    private void KeyInputUnit(EventType eventType, KeyCode key)
    {
        KeyInputType inputType = KeyInputType.None;
        if (Input.GetKeyDown(key)) inputType = KeyInputType.Down;
        else if (Input.GetKeyUp(key)) inputType = KeyInputType.Up;
        if(inputType == KeyInputType.None) return;
        EventBus.Instance.TriggerEvent<KeyInputType>(eventType, inputType);
    }
    private void ScreenInputDown(GameObject obj)
    {
        if (obj == null) return;
        if (obj.CompareTag("Track"))
        {
            EventBus.Instance.TriggerEvent<Track>(EventType.ScreenInputTrackDown, obj.GetComponent<Track>());
        }
    }
    private void ScreenInputUp(GameObject obj)
    {
        if (obj == null) return;
        if (obj.CompareTag("Track"))
        {
            EventBus.Instance.TriggerEvent<Track>(EventType.ScreenInputTrackUp, obj.GetComponent<Track>());
        }
    }
    private void OnDestroy()
    {
        StartGame = null;
        //PauseGame = null;
        UIManager.Instance.HideUI<GamePanel>();
    }
}
public enum KeyInputType
{
    None,
    Down,
    Up,
}

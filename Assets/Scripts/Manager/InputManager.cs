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
        _instance = this;
    }
    //public event UnityAction StartGame;
    //public event UnityAction PauseGame;
    private GamePanel gamePanel;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;
        if (Input.anyKeyDown/* && StartGame != null*/)
        {
            //StartGame += () => { StartGame = null; };
            //StartGame?.Invoke();
            EventBus.Instance.TriggerEvent(EventType.StartGame);
        }
        KeyInput();
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
        isRunning = true;
    }
    public void StopInput()
    {
        UIManager.Instance.HideUI<GamePanel>();
        isRunning = false;
    }
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            EventBus.Instance.TriggerEvent(EventType.PauseGame);

        if (Input.GetKeyDown(KeyCode.A))
            EventBus.Instance.TriggerEvent(EventType.Track_1_Down);
        if (Input.GetKeyUp(KeyCode.A))
            EventBus.Instance.TriggerEvent(EventType.Track_1_Up);

        if (Input.GetKeyDown(KeyCode.S))
            EventBus.Instance.TriggerEvent(EventType.Track_2_Down);
        if (Input.GetKeyUp(KeyCode.S))
            EventBus.Instance.TriggerEvent(EventType.Track_2_Up);

        if (Input.GetKeyDown(KeyCode.K))
            EventBus.Instance.TriggerEvent(EventType.Track_3_Down);
        if (Input.GetKeyUp(KeyCode.K))
            EventBus.Instance.TriggerEvent(EventType.Track_3_Up);

        if (Input.GetKeyDown(KeyCode.L))
            EventBus.Instance.TriggerEvent(EventType.Track_4_Down);
        if (Input.GetKeyUp(KeyCode.L))
            EventBus.Instance.TriggerEvent(EventType.Track_4_Up);
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
        //StartGame = null;
        //PauseGame = null;
        UIManager.Instance.HideUI<GamePanel>();
    }
}

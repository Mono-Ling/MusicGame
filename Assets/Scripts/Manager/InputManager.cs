using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
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
    public event UnityAction StartGame;
    public event UnityAction PauseGame;
    public event UnityAction TrackDown_1;
    public event UnityAction TrackUp_1;
    public event UnityAction TrackDown_2;
    public event UnityAction TrackUp_2;
    public event UnityAction TrackDown_3;
    public event UnityAction TrackUp_3;
    public event UnityAction TrackDown_4;
    public event UnityAction TrackUp_4;
    public event UnityAction<Track> ScreenInputTrackDown;
    public event UnityAction<Track> ScreenInputTrackUp;
    private GamePanel gamePanel;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowUI<GamePanel>();
        gamePanel = UIManager.Instance.GetUI<GamePanel>();
        gamePanel.InputDown += ScreenInputDown;
        gamePanel.InputUp += ScreenInputUp;
        gamePanel.InputPause += () => { PauseGame?.Invoke(); };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && StartGame != null)
        {
            StartGame += () => { StartGame = null; };
            StartGame?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) PauseGame?.Invoke();
        KeyInput();
    }
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            TrackDown_1?.Invoke();
        if (Input.GetKeyUp(KeyCode.A))
            TrackUp_1?.Invoke();

        if (Input.GetKeyDown(KeyCode.S))
            TrackDown_2?.Invoke();
        if (Input.GetKeyUp(KeyCode.S))
            TrackUp_2?.Invoke();

        if (Input.GetKeyDown(KeyCode.K))
            TrackDown_3?.Invoke();
        if (Input.GetKeyUp(KeyCode.K))
            TrackUp_3?.Invoke();

        if (Input.GetKeyDown(KeyCode.L))
            TrackDown_4?.Invoke();
        if (Input.GetKeyUp(KeyCode.L))
            TrackUp_4?.Invoke();
    }
    private void ScreenInputDown(GameObject obj)
    {
        if (obj == null) return;
        if (obj.CompareTag("Track"))
        {
            ScreenInputTrackDown?.Invoke(obj.GetComponent<Track>());
        }
    }
    private void ScreenInputUp(GameObject obj)
    {
        if (obj == null) return;
        if (obj.CompareTag("Track"))
        {
            ScreenInputTrackUp?.Invoke(obj.GetComponent<Track>());
        }
    }
    private void OnDestroy()
    {
        StartGame = null;
        PauseGame = null;
        UIManager.Instance.HideUI<GamePanel>();
    }
}

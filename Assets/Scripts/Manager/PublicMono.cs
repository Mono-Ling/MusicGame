using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PublicMono : MonoBehaviour
{
    private static PublicMono _instance;
    public static PublicMono Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PublicMono");
                _instance = go.AddComponent<PublicMono>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    public event UnityAction updateAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateAction?.Invoke();
    }
    private void OnDestroy()
    {
        updateAction = null;
    }
}

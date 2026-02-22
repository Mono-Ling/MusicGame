using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        { 
            if (_instance == null)
            {
                GameObject obj = new GameObject($"{typeof(AudioManager)}");
                _instance = obj.AddComponent<AudioManager>();
                DontDestroyOnLoad(obj);
            }    
            return _instance; 
        }
    }
    private float musicVolume = 1.0f;
    private AudioSource musicSource;
    private void Awake()
    {
        musicVolume = SettingManager.Instance.settingData.musicVolume;
        musicSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //musicVolume = SettingManager.Instance.settingData.musicVolume;
    }
    public void SetMusic(AudioClip clip, bool loop = false)
    {
        if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = loop;
        musicSource.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (musicSource != null) musicSource.volume = musicVolume;
    }
    public void PlayMusic()
    {
        if (musicSource != null) musicSource.Play();
        else Debug.LogError("ÒôÀÖ²¥·ÅÆ÷Îª¿Õ£¡");
    }
    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }
}

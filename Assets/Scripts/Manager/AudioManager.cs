using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning($"µ¥Àý{_instance}ÖØ¸´×¢²á");
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    private float musicVolume = 1.0f;
    public AudioSource musicSource { get; private set; }
    
    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        SetMusicVolume(SettingManager.Instance.settingData.musicVolume);
        //musicVolume = SettingManager.Instance.settingData.musicVolume;
    }
    public void SetMusic(AudioClip clip,bool loop = false)
    {
        if(musicSource == null) musicSource = GetComponent<AudioSource>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicView : MonoBehaviour
{
    private static MusicView instance;
    public static MusicView Instance => instance;
    private void Awake()
    {
        instance = this;
    }
    public AudioSource audioSource;
    public int spectrumSize = 1024;
    public FFTWindow window = FFTWindow.Blackman;
    float[] spectrumData;
    float[] frequencyBands = new float[8];
    int[] bandLimits = new int[9] { 20, 63, 160, 400, 1000, 2500, 6300, 10000, 20000 };//原20, 60, 150, 250, 500, 1000, 2000, 4000, 20000
    float[] smoothedBands = new float[8];
    public float smoothSpeed = 10f;
    public float[] normalizedBands = new float[8];
    public float min = 0;
    public float max = 0.1f;
    public Material material;
    public bool isNormalize = true;
    public int updateInterval = 5;
    private Camera mainCamera;
    private Color lowColor;
    private Color highColor;
    private int frameCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        float scale = mainCamera.orthographicSize * 2 * mainCamera.aspect / 6f; //screenStep.x / 100f;
        transform.localScale = new Vector3(scale * 4,scale * 2, 1);
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0));
        ColorData[] colorDatas = SelectLevelManager.Instance.GetBKColor();
        ColorData color = colorDatas[0];
        lowColor = new Color(color.r, color.g,color.b,color.a);
        color = colorDatas[1];
        highColor = new Color(color.r, color.g, color.b, color.a);
        material.SetColor("_Color", lowColor);
        material.SetColor ("_MaxColor", highColor);
        spectrumData = new float[spectrumSize];
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null) audioSource = AudioManager.Instance.musicSource;
        if (audioSource == null) return;

        if(frameCount++ > updateInterval)
        {
            CalculateFrequencyBands();
            frameCount = 0;
        }
        
        SmoothBands();
        NormalizedZero2One();

        if (material != null)
        {
            if (isNormalize)
                material.SetFloatArray("_Audio", normalizedBands);
            else
                material.SetFloatArray("_Audio", smoothedBands);
        }
    }
    void CalculateFrequencyBands()
    {
        //spectrumData = new float[spectrumSize];
        audioSource.GetSpectrumData(spectrumData, 0, window);
        System.Array.Clear(frequencyBands, 0, frequencyBands.Length);
        // 3. 将频谱数据映射到8个频段
        int sampleRate = AudioSettings.outputSampleRate;
        float freqPerBin = sampleRate / 2f / spectrumSize;

        for (int i = 0; i < spectrumSize; i++)
        {
            float frequency = i * freqPerBin;

            // 确定当前频率属于哪个频段
            for (int band = 0; band < 8; band++)
            {
                if (frequency >= bandLimits[band] && frequency < bandLimits[band + 1])
                {
                    // 累加该频段的能量
                    frequencyBands[band] += spectrumData[i];
                    break;
                }
            }
            //for (int band = 0; band < 8; band++)
            //{
            //    frequencyBands[band] /= 8;
            //}
        }
    }
    void SmoothBands()
    {
        for (int i = 0; i < 8; i++)
        {
            smoothedBands[i] = Mathf.Lerp(smoothedBands[i], frequencyBands[i], Time.deltaTime * smoothSpeed);
        }
    }
    void NormalizedZero2One()
    {
        for (int i = 0; i < 8; i++)
        {
            // 将 smoothedBands 映射到 0~1 区间（基于设置的 min 和 max）
            normalizedBands[i] = Mathf.InverseLerp(min, max, smoothedBands[i]);
            // 限制最大值为1，避免超出范围
            normalizedBands[i] = Mathf.Clamp01(normalizedBands[i]);
        }
    }
}

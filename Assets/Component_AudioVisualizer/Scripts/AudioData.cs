using UnityEngine;
using UnityEngine.UI;

public class AudioData : MonoBehaviour
{
    [SerializeField] [Tooltip("Source of audio to visualize")] private AudioSource sourceAudio;
    [SerializeField] [Tooltip("Scale the size of the bands")] [Range(1f, 5f)] private float scale = 1f;

    private int numberOfSamples = 1024; //Divide the full frequencies (20kHz) into this number of slices. Must be power of 2. Min 64 Max 8192
    private Slider[] sliders = new Slider[3];
    private float[] samples;

    private float[] freqBand = new float[3];
    private float[] bandBuffer = new float[3];
    private float[] bufferDecrease = new float[3];

    private void Start()
    {
        samples = new float[numberOfSamples];

        AssignBands();
    }

    private void Update()
    {
        if (sourceAudio != null)
        {
            GetSpectrumAudioSource();
            MakeFrenquencyBands();
            BandBuffer();
        }
    }

    private void AssignBands()
    {
        //Assign Bands and Sliders GameObject to Array
        for (int i = 0; i < 3; i++)
        {
            sliders[i] = transform.GetChild(i).GetComponent<Slider>();
        }
    }

    private void GetSpectrumAudioSource()
    {
        sourceAudio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void BandBuffer()
    {
        for (int g = 0; g < 3; g++)
        {
            if (freqBand[g] > bandBuffer[g])
            {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.005f;
            }
            if (freqBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
            sliders[g].value = bandBuffer[g];
        }
    }

    private void MakeFrenquencyBands()
    {
        // 23.4 Htz per sample (24 000 / 1024)

        int count = 0;

        for (int i = 0; i < 3; i++)
        {
            float average = 0;
            //int sampleCount = (int)Mathf.Pow(2, i) * 2;
            int sampleCount = (int)Mathf.Pow(3.5f, i) * 50;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;

            freqBand[i] = average * scale;
        }
    }
}
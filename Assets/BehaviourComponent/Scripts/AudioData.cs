using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioData : MonoBehaviour
{
    [SerializeField] private int numberOfSamples; //Divide the full frequencies (20kHz) into this number of slices. Must be power of 2. Min 64 Max 8192
    
    public static float[] samples;

    private AudioSource audioSource;
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];

    private float[] bufferDecrease = new float[8];

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        samples = new float[numberOfSamples];
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrenquencyBands();
        BandBuffer();
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
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
        }
    }

    private void MakeFrenquencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count +1);
                count ++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioData : MonoBehaviour
{
    private int numberOfSamples = 1024; //Divide the full frequencies (20kHz) into this number of slices. Must be power of 2. Min 64 Max 8192
    
    public static float[] samples;

    private AudioSource audioSource;
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] freqBandHighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude, amplitudeBuffer;
    private float amplitudeHighest;

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
        CreateAudioBands();
        GetAmplitude();
    }

    private void GetAmplitude()
    {
        float currentAmplitude = 0f;
        float currentAmplitudeBuffer = 0f;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
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

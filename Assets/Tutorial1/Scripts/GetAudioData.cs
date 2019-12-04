using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

[RequireComponent (typeof (AudioSource))]
public class GetAudioData : MonoBehaviour
{
    [SerializeField] private int numberOfSamples = 1024; //Divide the full frequencies (20kHz) into this number of slices. Must be power of 2. Min 64 Max 8192
    [SerializeField] private float[] samples;

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        samples = new float[numberOfSamples];
    }

    private void Update()
    {
        GetSpectrumAudioSource();
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}

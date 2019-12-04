using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAmplitude : MonoBehaviour
{
    [SerializeField] private float minimumAmplitude, multiplier;
    
    private Light light;

    private void Start() 
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = (AudioData.amplitude * multiplier) + minimumAmplitude;
    }
}

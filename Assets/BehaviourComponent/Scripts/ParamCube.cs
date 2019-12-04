using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    [SerializeField] private int band;
    [SerializeField] private float startScale, scaleMultiplier;
    [SerializeField] private bool useBuffer;

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioData.bandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioData.freqBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
    }
}
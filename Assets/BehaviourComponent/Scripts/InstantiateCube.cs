using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCube : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float maxScale;
    
    private GameObject[] sampleCube = new GameObject[512];

    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject instanceSampleCube = (GameObject)Instantiate (cubePrefab);
            instanceSampleCube.transform.position = this.transform.position;
            instanceSampleCube.transform.parent = this.transform;
            instanceSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3( 0, -0.703125f * i, 0);
            instanceSampleCube.transform.position = Vector3.forward * 100;
            sampleCube[i] = instanceSampleCube;
        }
    }

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (sampleCube != null)
            {
                sampleCube[i].transform.localScale = new Vector3(1, AudioData.samples[i] * maxScale +2, 1);
            }
        }
    }
}

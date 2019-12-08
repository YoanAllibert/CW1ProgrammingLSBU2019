using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, objectToFollow.position + offset, ref velocity, smoothTime);
    }
}
using UnityEngine;
using System.Collections;

public class DollyZoom : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Camera cam;
    private float initialFrustrumWidth;

    void Awake()
    {
        cam = GetComponent<Camera>();

        float initialDistance = Vector3.Distance(transform.position, target.position);
        initialFrustrumWidth = CalculateFrustrumWidth(initialDistance);
    }

    void Update()
    {
        transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * Time.deltaTime * 5f);

        float curentDistance = Vector3.Distance(transform.position, target.position);
        cam.fieldOfView = CalculateFieldOfView(initialFrustrumWidth, curentDistance);
    }

    private float CalculateFrustrumWidth(float distance)
    {
        return (2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad));
    }

    private float CalculateFieldOfView(float width, float distance)
    {
        return (2f * Mathf.Atan(width * 0.5f / distance) * Mathf.Rad2Deg);
    }
}
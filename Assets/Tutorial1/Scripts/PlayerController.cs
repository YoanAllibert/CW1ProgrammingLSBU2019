using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody rb;
    private float verticalInput;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.AddForce(horizontalInput * speed * Time.deltaTime, 0f, verticalInput * speed * Time.deltaTime, ForceMode.Impulse);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class Player : MonoBehaviour
{
    // Movement Look
    public float speed = 6.0f;
    float rotationX = 0f;
    float rotationY = 0f;
    public float sensitivity = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        // Move around
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(xInput, 0, yInput).normalized;
        transform.Translate(speed * Time.deltaTime * moveDirection);

        // Look around
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}

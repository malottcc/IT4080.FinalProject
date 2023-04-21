using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;


namespace It4080
{
    public class Player : NetworkBehaviour
    {
        // Movement Look
        public float speed = 6.0f;
        float rotationX = 0f;
        float rotationY = 0f;
        public float sensitivity = 1f;

        // Jump
        public Rigidbody rb;

        // Camera
        private Camera camera;

        // Loot Value
        public NetworkVariable<int> wagonScore = new NetworkVariable<int>(0);

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            rb = GetComponent<Rigidbody>();
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

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector3(0, 5, 0);
            }
        }

        //-------------------------------
        //Network Spawn

        public override void OnNetworkSpawn()
        {
            camera = transform.Find("Camera").GetComponent<Camera>();
            camera.enabled = IsOwner;
        }
    }
}

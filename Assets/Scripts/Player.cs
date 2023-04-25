using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;
using It4080;


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

        // Wagon Value
        public NetworkVariable<int> wagonScore = new NetworkVariable<int>();
        public int StartWagonScore = 0;

        // Loot Value
        public NetworkVariable<int> playerLootScore = new NetworkVariable<int>();
        public NetworkVariable<bool> isCarryingLoot = new NetworkVariable<bool>();
        public int StartLootValue = 0;
        public bool Switch = false;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            rb = GetComponent<Rigidbody>();
        }

        //-------------------------------
        //Network Spawn

        public override void OnNetworkSpawn()
        {
            camera = transform.Find("Camera").GetComponent<Camera>();
            camera.enabled = IsOwner;

            playerLootScore.Value = StartLootValue;
            wagonScore.Value = StartWagonScore;
        }

        //---------------------------------
        // Pick Up Loot

        void OnCollisionEnter(Collision collision)
        {

            if (IsServer)
            {
                if (collision.gameObject.CompareTag("YellowStar"))
                {
                    ServerHandleYellowStarPickUp(collision.gameObject);
                }

                if (collision.gameObject.CompareTag("BlueDiamond"))
                {
                    ServerHandleBlueDiamondPickUp(collision.gameObject);
                }
            }
        }

        public void ChangeStateCarryingLoot(bool previous, bool current)
        {

        }

        private void ServerHandleYellowStarPickUp(GameObject destroyStar)
        {
            Debug.Log("Picked Up Star");
            playerLootScore.Value += 15;
            Destroy(destroyStar);
        }

        private void ServerHandleBlueDiamondPickUp(GameObject destroyStar)
        {
            Debug.Log("Picked Up Diamond");
            playerLootScore.Value += 25;
            Destroy(destroyStar);
        }

        public void ClearPlayerLootValue()
        {
            playerLootScore.Value = 0;
            Debug.Log("Points Back to Zero");
            Debug.Log(playerLootScore.Value);
        }


        //----------------------
        // Update
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

            // Move Players
            if (IsOwner)
            {
                //PlayerMovementServerRpc(moveDirection);
            }
        }

        //------------------------------
        //Server Move Players

        [ServerRpc]
        public void PlayerMovementServerRpc(Vector3 posChange, Vector3 posRotate, ServerRpcParams rpcParams = default)
        {
            transform.Translate(posChange);
            transform.Translate(posRotate);
        }
    }
}

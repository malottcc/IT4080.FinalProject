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
        public float speed = 4.5f;
        float rotationX = 0f;
        float rotationY = 0f;
        public float sensitivity = 3.5f;

        // Jump
        public Rigidbody rb;
        public Vector3 grounded;

        // Camera
        private Camera camera;

        // Wagon Value
        public NetworkVariable<int> wagonScore = new NetworkVariable<int>();
        public int StartWagonScore = 0;

        // Loot Value
        public NetworkVariable<int> playerLootScore = new NetworkVariable<int>();
        public NetworkVariable<bool> isCarryingLoot = new NetworkVariable<bool>();
        public int StartLootValue = 0;
        public bool StartNotCarryingLoot = false;

        // ClientId
        public int clientid;

        void Start()
        {
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
            isCarryingLoot.Value = StartNotCarryingLoot;
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

                if (collision.gameObject.CompareTag("GreenHorcrux"))
                {
                    ServerHandleGreenHorcruxPickUp(collision.gameObject);
                }

                if (collision.gameObject.CompareTag("RedSphere"))
                {
                    ServerHandleRedSpherePickUp(collision.gameObject);
                }

                if (collision.gameObject.CompareTag("Wizard"))
                {
                    KickPlayerOutOfHouse();
                }
            }
        }

        private void ServerHandleYellowStarPickUp(GameObject destroyStar)
        {
            if (isCarryingLoot.Value == true)
            {
                return;
            }
            else
            {
                Debug.Log("Picked Up Star");
                playerLootScore.Value += 15;
                isCarryingLoot.Value = true;
                Destroy(destroyStar);
            }
        }

        private void ServerHandleGreenHorcruxPickUp(GameObject destoryHorcrux)
        {
            if (isCarryingLoot.Value == true)
            {
                return;
            }
            else
            {
                Debug.Log("Picked Up Horcrux");
                playerLootScore.Value += 40;
                isCarryingLoot.Value = true;
                Destroy(destoryHorcrux);
            }

        }

        private void ServerHandleBlueDiamondPickUp(GameObject destroyStar)
        {
            if (isCarryingLoot.Value == true)
            {
                return;
            }
            else
            {
                Debug.Log("Picked Up Diamond");
                playerLootScore.Value += 25;
                isCarryingLoot.Value = true;
                Destroy(destroyStar);
            }

        }

        private void ServerHandleRedSpherePickUp(GameObject destroyStar)
        {
            if (isCarryingLoot.Value == true)
            {
                return;
            }
            else
            {
                Debug.Log("Picked Up Sphere");
                playerLootScore.Value += 100;
                isCarryingLoot.Value = true;
                Destroy(destroyStar);
            }

        }

        public void ClearPlayerLootValue()
        {
            playerLootScore.Value = 0;
            Debug.Log("Points Back to Zero");
            Debug.Log(playerLootScore.Value);
        }

        public void ResetCarryingLoot()
        {
            isCarryingLoot.Value = false;
        }

        //--------------------------------------
        // Wizard Collided

        public void KickPlayerOutOfHouse()
        {
            BootOutServerRpc();
            ClearPlayerLootValue();
            ResetCarryingLoot();
        }

        [ServerRpc]
        public void BootOutServerRpc(ServerRpcParams rpcParams = default)
        {
            BootOutClientRpc();
        }

        [ClientRpc]
        public void BootOutClientRpc(ClientRpcParams rpcParams = default)
        {
            transform.position = new Vector3(2, 1, 23);
            Debug.Log("Ye got got");
        }

        //-------------------------------
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
            if (Input.GetKeyDown(KeyCode.T))
            { 
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            // Jump
            grounded = gameObject.transform.position;
            if (Input.GetKeyDown(KeyCode.Space) && grounded.y < 0)
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
        // Server Move Players

        [ServerRpc]
        public void PlayerMovementServerRpc(Vector3 posChange, Vector3 posRotate, ServerRpcParams rpcParams = default)
        {
            transform.Translate(posChange);
            transform.Translate(posRotate);
        }
    }
}

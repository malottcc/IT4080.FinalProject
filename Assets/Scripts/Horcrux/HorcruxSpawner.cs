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

    public class HorcruxSpawner : NetworkBehaviour
    {
        public GameObject greenHorcrux;
        public bool spawnOnLoad = true;
        public GameObject curGreenHorcrux = null;
        public float timeRemaining = 0f;
        public float refreshTime = 2f;

        public void Start()
        {

        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                HostOnNetworkSpawn();
            }
        }

        private void HostOnNetworkSpawn()
        {
            SpawnGreenHorcruxServerRpc();
        }

        void OnCollisionEnter(Collision collision)
        {

        }

        void Update()
        {
            if (IsServer)
            {
                if (timeRemaining > 0f)
                {
                    timeRemaining -= Time.deltaTime;
                    if (timeRemaining <= 0f)
                    {
                        timeRemaining = 0;
                        SpawnGreenHorcruxServerRpc();
                    }
                }
                else if (curGreenHorcrux == null)
                {
                    timeRemaining = refreshTime;
                }
            }
        }

        [ServerRpc]
        public void SpawnGreenHorcruxServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Green Horcrux");
            GameObject InstansiatedGreenHorcrux = Instantiate(greenHorcrux, gameObject.transform.position, UnityEngine.Quaternion.identity);
            InstansiatedGreenHorcrux.GetComponent<NetworkObject>().Spawn();
            curGreenHorcrux = InstansiatedGreenHorcrux;
        }
    }
}

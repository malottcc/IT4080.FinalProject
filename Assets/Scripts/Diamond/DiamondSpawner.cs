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
    public class DiamondSpawner : NetworkBehaviour
    {

        public GameObject blueDiamond;
        public bool spawnOnLoad = true;
        public GameObject curBlueDiamond = null;
        public float timeRemaining = 0f;
        public float refreshTime = 50f;

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
            SpawnBlueDiamondServerRpc();
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
                        SpawnBlueDiamondServerRpc();
                    }
                }
                else if (curBlueDiamond == null)
                {
                    timeRemaining = refreshTime;
                }
            }
        }

        [ServerRpc]
        public void SpawnBlueDiamondServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Blue Diamond");
            GameObject InstansiatedBlueDiamond = Instantiate(blueDiamond, gameObject.transform.position, UnityEngine.Quaternion.identity);
            InstansiatedBlueDiamond.GetComponent<NetworkObject>().Spawn();
            curBlueDiamond = InstansiatedBlueDiamond;
        }
    }
}

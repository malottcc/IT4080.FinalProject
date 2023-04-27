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
    public class StarSpawner : NetworkBehaviour
    {

        public GameObject yellowStar;
        public bool spawnOnLoad = true;
        public GameObject curYellowStar = null;
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
            SpawnYellowStarServerRpc();
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
                        SpawnYellowStarServerRpc();
                    }
                }
                else if (curYellowStar == null)
                {
                    timeRemaining = refreshTime;
                }
            }
        }

        [ServerRpc]
        public void SpawnYellowStarServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Yellow Star");
            GameObject InstansiatedYellowStar = Instantiate(yellowStar, gameObject.transform.position, UnityEngine.Quaternion.identity);
            InstansiatedYellowStar.GetComponent<NetworkObject>().Spawn();
            curYellowStar = InstansiatedYellowStar;
        }
    }
}

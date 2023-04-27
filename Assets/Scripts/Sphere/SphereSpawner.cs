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

    public class SphereSpawner : NetworkBehaviour
    {
        public GameObject redSphere;
        public bool spawnOnLoad = true;
        public GameObject curRedSphere = null;
        public float timeRemaining = 0f;
        public float refreshTime = 120f;

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
            SpawnRedSphereServerRpc();
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
                        SpawnRedSphereServerRpc();
                    }
                }
                else if (curRedSphere == null)
                {
                    timeRemaining = refreshTime;
                }
            }
        }

        [ServerRpc]
        public void SpawnRedSphereServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Green Horcrux");
            GameObject InstansiatedRedSphere = Instantiate(redSphere, gameObject.transform.position, UnityEngine.Quaternion.identity);
            InstansiatedRedSphere.GetComponent<NetworkObject>().Spawn();
            curRedSphere = InstansiatedRedSphere;
        }
    }
}
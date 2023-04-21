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
    public class LootSpawner : NetworkBehaviour
    {

        public UnityEngine.Vector3 SpawnPosition;
        public GameObject blueDiamond;
        public bool spawnOnLoad = true;

        public void Start()
        {
            SpawnPosition = gameObject.transform.position;
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

        void Update()
        {

        }

        [ServerRpc]
        public void SpawnBlueDiamondServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Blue Diamond");
            GameObject InstansiatedBlueDiamond = Instantiate(blueDiamond, SpawnPosition, UnityEngine.Quaternion.identity);
            InstansiatedBlueDiamond.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        }
    }
}

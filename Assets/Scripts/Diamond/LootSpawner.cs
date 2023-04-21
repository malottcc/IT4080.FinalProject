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

        public GameObject blueDiamond;
        //public GameObject yellowStar;
        public bool spawnOnLoad = true;

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
            //SpawnYellowStarServerRpc();
        }

        void Update()
        {

        }

        [ServerRpc]
        public void SpawnBlueDiamondServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Spawning Blue Diamond");
            GameObject InstansiatedBlueDiamond = Instantiate(blueDiamond, gameObject.transform.position, UnityEngine.Quaternion.identity);
            InstansiatedBlueDiamond.GetComponent<NetworkObject>().Spawn();
        }
    }
}

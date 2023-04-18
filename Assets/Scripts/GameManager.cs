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
    public class GameManager : NetworkBehaviour
    {

        public Player playerPrefab;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                SpawnAllPlayers();
            }
        }


        private void SpawnAllPlayers()
        {
            Debug.Log("Spawing Players");
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnPlayerForClient(clientId);
            }
        }

        private Player SpawnPlayerForClient(ulong clientId)
        {
            Debug.Log("Spawing Clients");
            Vector3 spawnPosition = new Vector3(1 + clientId * 5, 1, 18);
            Player playerSpawn = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            return playerSpawn;
        }

    }
}
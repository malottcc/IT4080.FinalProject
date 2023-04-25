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
        public Wagon wagonPrefab;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                SpawnAllPlayers();
                SpawnAllWagons();
                NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnected;
            }
        }

        public void update()
        {

        }

        private void ServerOnClientConnected(ulong clientId)
        {
            Player newPlayer = SpawnPlayerForClient(clientId);
            Wagon newWagon = SpawnWagonForClient(clientId);
            newWagon.player = newPlayer;
        }

        private void SpawnAllPlayers()
        {
            Debug.Log("Spawning Players");
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnPlayerForClient(clientId);
            }
        }

        private void SpawnAllWagons()
        {
            Debug.Log("Spawning Wagons");
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnWagonForClient(clientId);
            }
        }

        private Player SpawnPlayerForClient(ulong clientId)
        {
            Debug.Log("Spawning Clients");
            Vector3 spawnPosition = new Vector3(1 + clientId * 5, 1, 18);
            Player playerSpawn = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            Debug.Log("ClientID is - " + clientId);
            return playerSpawn;
        }


        private Wagon SpawnWagonForClient(ulong clientId)
        {
            Debug.Log("Spawning Wagons");
            Vector3 spawnPosition = new Vector3(1 + clientId * 8, -4, 16);
            Wagon wagonSpawn = Instantiate(wagonPrefab, spawnPosition, Quaternion.identity);
            wagonSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            return wagonSpawn;
        }

    }
}
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
                SpawnAllPlayersAndWagons();
                NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnected;
            }
        }

        public void update()
        {

        }

        private void ServerOnClientConnected(ulong clientId)
        {
            Spawnstuff(clientId);
        }

        private void Spawnstuff(ulong clientId)
        {
            Player newPlayer = SpawnPlayerForClient(clientId);
            Wagon newWagon = SpawnWagonForClient(clientId);
            newWagon.player = newPlayer;
        }

        private void SpawnAllPlayersAndWagons()
        {
            Debug.Log("Spawning Players");
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Spawnstuff(clientId);
            }
        }


        private Player SpawnPlayerForClient(ulong clientId)
        {
            Debug.Log("Spawning Clients");
            Vector3 spawnPosition = new Vector3(1 + clientId * 5, 1, 23);
            Player playerSpawn = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            Debug.Log("ClientID is - " + clientId);
            return playerSpawn;
        }


        private Wagon SpawnWagonForClient(ulong clientId)
        {
            Debug.Log("Spawning Wagons");
            Vector3 spawnPosition = new Vector3(1 + clientId * 8, -4, 23);
            Wagon wagonSpawn = Instantiate(wagonPrefab, spawnPosition, Quaternion.identity);
            wagonSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            Debug.Log("Wagon Id is - " + clientId);
            return wagonSpawn;
        }

    }
}
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
    public class Wagon : NetworkBehaviour
    {

        public TMP_Text worldText;
        public string displayScore;
        public It4080.Player player;
        public int WagonCurrentScore = 0;

        void Start()
        {
            worldText.text = "Value = 0";
        }

        void OnCollisionEnter(Collision collider)
        {
            if (IsServer)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Wagon Collided");
                    ServerOnWagonCollision(collider.gameObject.GetComponent<Player>());
                }
                
            }
        }

        void ServerOnWagonCollision(Player collidePlayer)
        {
            //collidePlayer = player;

            if (IsOwner)
            {
                Debug.Log("Player that collided ID - " + NetworkManager.Singleton.LocalClientId);
                collidePlayer.wagonScore.Value += collidePlayer.playerLootScore.Value;
                WagonCurrentScore = collidePlayer.wagonScore.Value;
                ChangeWagonScoreClientRpc(WagonCurrentScore);
                collidePlayer.ClearPlayerLootValue();
            }
        }

        /*
        [ServerRpc(RequireOwnership = false)]
        public void ChangeWagonScoreServerRpc(int score, ServerRpcParams serverRpcParams = default)
        { 
            Debug.Log("Server Change Score");
            ChangeWagonScoreClientRpc(score);
        }

        if (collidePlayer != player)
            {
                return;
            }
        */

        [ClientRpc]
        public void ChangeWagonScoreClientRpc(int wagonScore, ClientRpcParams rpcParams = default)
        {
            worldText.text = "Value = " + wagonScore;
            Debug.Log("Points added to Wagon");
        }

        void Update()
        {

        }
    }
}
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
        private ulong clientId = 0;

        void Start()
        {
            worldText.text = "Value = 0";
        }

        void OnCollisionEnter(Collision collider)
        {
            if (IsOwner)
            {
                Debug.Log("Wagon Collided");
                ServerOnWagonCollision(collider.gameObject.GetComponent<Player>());
            }
        }

        void ServerOnWagonCollision(Player collidePlayer)
        {
            if (IsOwner)
            {
                clientId = NetworkManager.Singleton.LocalClientId;
                collidePlayer.wagonScore.Value += collidePlayer.playerLootScore.Value;
                WagonCurrentScore = collidePlayer.wagonScore.Value;
                ChangeWagonScoreClientRpc(WagonCurrentScore);
                collidePlayer.ClearPlayerLootValue();
            }
            
        }

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
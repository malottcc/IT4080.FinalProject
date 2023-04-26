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
                    Debug.Log("Player that collided ID - " + NetworkManager.Singleton.LocalClientId);
                    ServerOnWagonCollision(collider.gameObject.GetComponent<Player>());
                }
                
            }
        }

        void ServerOnWagonCollision(Player collidePlayer)
        {
            Debug.Log("Player that collided ID - " + NetworkManager.Singleton.LocalClientId);

            if (collidePlayer == player)
            {
                collidePlayer.wagonScore.Value += collidePlayer.playerLootScore.Value;
                ChangeWagonScoreClientRpc(collidePlayer.wagonScore.Value);
                collidePlayer.ClearPlayerLootValue();
                collidePlayer.ResetCarryingLoot();
            }
        }

        [ClientRpc]
        public void ChangeWagonScoreClientRpc(int wagonScore, ClientRpcParams rpcParams = default)
        {
            worldText.text = "Value = " + wagonScore;
            Debug.Log("Points added to Wagon");
        }
    }
}
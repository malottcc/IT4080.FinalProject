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
    public class Timer : NetworkBehaviour
    {
        public bool timerOn = false;
        public float timeLeft = 180;
        public TMP_Text worldText;

        public override void OnNetworkSpawn()
        {
            worldText.text = "0:00";
        }

        void Start()
        {
            worldText.text = "0:00";
        }

        void OnTriggerEnter(Collider collider)
        {
            if (IsServer)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    StartTimer();
                }

            }
        }

        void StartTimer()
        {
            timerOn = true;
        }

        void Update()
        {

            if (timerOn == true)
            {
                if (timeLeft > 0)
                {
                    Debug.Log("Times running out");
                    timeLeft -= Time.deltaTime;
                    updateTimer(timeLeft, timerOn);
                }
                else
                {
                    timerOn = false;
                    updateTimer(timeLeft, timerOn);

                }
            }
        }

        void updateTimer(float currentTime, bool timerOn)
        {
            currentTime += 1;

            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);

            ChangeTimerClientRpc(minutes, seconds, timerOn);
        }

        [ClientRpc]
        public void ChangeTimerClientRpc(float minute, float second, bool timerDone, ClientRpcParams rpcParams = default)
        {
            if (timerDone == true)
            {
                worldText.text = minute + ":" + second;
            }
            else
            {
                Debug.Log("Game Over!");
                worldText.text = "Game!";
            }
        }

    }
}
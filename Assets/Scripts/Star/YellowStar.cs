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

    public class YellowStar : NetworkBehaviour
    {
        public GameObject curStar;

        public override void OnNetworkSpawn()
        {
            Vector3 newRotation = new Vector3(-90, 0, 0);
            transform.eulerAngles = newRotation;
        }


        void Start()
        {

        }

        void Update()
        {
            transform.Rotate(0f, 0f, 50f * Time.deltaTime, Space.Self);
        }

        void OnCollisionEnter(Collision collision)
        {

        }


    }
}

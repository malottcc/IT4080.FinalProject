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
    public class InGameUI : NetworkBehaviour
    {

        public Behaviour Canvas;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Canvas.enabled = !Canvas.enabled;
            }
        }
    }
}

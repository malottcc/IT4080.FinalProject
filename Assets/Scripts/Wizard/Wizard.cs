using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.AI;


namespace It4080
{
    public class Wizard : NetworkBehaviour
    {
        public NavMeshAgent navMeshAgent;
        [SerializeField] private Transform movePositionTransform;

        public void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Update()
        {
            navMeshAgent.destination = movePositionTransform.position;
        }
    }
}

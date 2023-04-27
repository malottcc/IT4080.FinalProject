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
        public It4080.Player player;
        public NavMeshAgent navMeshAgent;
        public Transform[] stepPoints;
        int stepPointIndex;
        public Vector3 target;
        public Transform kickedOut;
   

        public void Start()
        {
            UpdateDestination();
        }

        public void Update()
        {
            if (Vector3.Distance(transform.position, target) < 1)
            {
                IterateStepPointIndex();
                UpdateDestination();
            }
        }

        void UpdateDestination()
        {
            target = stepPoints[stepPointIndex].position;
            navMeshAgent.SetDestination(target);
        }

        void IterateStepPointIndex()
        {
            stepPointIndex++;
            if (stepPointIndex == stepPoints.Length)
            {
                stepPointIndex = 0;
            }
        }
    }
}

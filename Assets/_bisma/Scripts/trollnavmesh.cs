using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class trollnavmesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
      navMeshAgent= GetComponent<NavMeshAgent>(); 
    }
    private void Update()
    {
        navMeshAgent.destination=movePositionTransform.position;
    }
    

    
   
}


using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Troll : MonoBehaviour
{
    [HideInInspector]
    public Transform Gem = null;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private bool Gemspawned = false;

    MachineController controller;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        points = TrollSpawner.instance.spawnPoints;
        controller = FindObjectOfType<MachineController>();
        
        agent.autoBraking = false;
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        

        if (!Gemspawned)
        {
            agent.destination = points[destPoint].position;
        }
        else
        {
            agent.destination = RandomGem();
        }
        destPoint = (destPoint + 1) % points.Length;
    }

    private Vector3 RandomGem()
    {
        List<GameObject> gems = new List<GameObject>();
        gems.AddRange(GameObject.FindGameObjectsWithTag("ChunkGem"));
        gems.RemoveAll(x => x == null);
        Vector3 gem = gems[Random.Range(0, gems.Count - 1)].transform.position;
        return gem;
    }

    void Update ()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

        if (GameObject.FindGameObjectWithTag("ChunkGem") != null)
        {
            Gemspawned = true;
        }
        else
        {
            Gemspawned = false;
            Gem = null;
        }
    }

    public void DestroyTroll()
    {
        Destroy(gameObject);
    }
}
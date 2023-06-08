using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Patrol : MonoBehaviour
{
    [HideInInspector]
    public Transform Gem = null;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private bool Gemspawned = false; 

    void Start ()
    {
        

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        Debug.Log("Next waypoint");

        // Returns if no points have been set up
        if (points.Length == 0)
            return;
        

        // Set the agent to go to the currently selected destination.
        if (!Gemspawned)
        {
            agent.destination = points[destPoint].position;
        }
        else
        {
            //Gem = GameObject.FindGameObjectWithTag("ChunkGem").transform;
            //agent.destination = Gem.position;
            agent.destination = RandomGem();
        }

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    private Vector3 RandomGem()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("ChunkGem");
        Vector3 gem = gems[Random.Range(0, gems.Length - 1)].transform.position;
        return gem;
    }

    void Update ()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

        if (GameObject.FindGameObjectWithTag("ChunkGem") != null)
        {
            //Activate if statemant when cave colliders are in place
            //Gemspawned = true;

        }
        else
        {
            Gemspawned = false;
            Gem = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ChunkGem")
        {
            Destroy(collision.gameObject);
        }
    }

    public void DestroyTroll()
    {
        Destroy(gameObject);
    }
}
  using UnityEngine;
    using UnityEngine.AI;
    using System.Collections;


    public class Patrol : MonoBehaviour {
    [HideInInspector]
        public Transform Gem=null;
        public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;
       [SerializeField] private bool Gemspawned = false; 


        void Start () {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            GotoNextPoint();
        }


        void GotoNextPoint() {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;
        agent.destination = points[destPoint].position;

        // Set the agent to go to the currently selected destination.
        if (!Gemspawned)
        {
            //agent.destination = points[destPoint].position;
        }
            else
        {
            Gem.position = GameObject.FindGameObjectWithTag("ChunkGem").transform.position;
            //agent.destination = Gem.position;
        }

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }


        void Update () {
           
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();

        if (GameObject.FindGameObjectsWithTag("ChunkGem") != null)
        {
            Gemspawned = true;
        }
        else Gemspawned = false;
        Debug.Log(agent.remainingDistance);

    }
            
    }
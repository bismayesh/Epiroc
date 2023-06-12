using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Troll : MonoBehaviour
{
    public GameObject trollParent;
    public List<GameObject> gems = new List<GameObject>();
    public List<Material> gemMaterials = new List<Material>();
    public Renderer gemRenderer;
    public Transform gemPosition;
    public float detectDistance;
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    public AudioSource audioSource;
    public AudioClip dyingClip;

    private NavMeshAgent agent;
    private int gemIndex;
    private int colorIndex;

    public bool killTroll = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetGemIndex();
        gemRenderer.material = gemMaterials[GetColorIndex()];
    }

    int GetGemIndex()
    {
        return gemIndex = Random.Range(0, gems.Count - 1);
    }

    int GetColorIndex()
    { 
        return Random.Range(0, gemMaterials.Count);
    }


    private void Update()
    {
        if (agent.remainingDistance < 0.1f)
        {
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (killTroll)
        {
            killTroll = false;
            DestroyTroll();
        }
    }

    void WanderToNewLocation()
    {

        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    public void DestroyTroll()
    {
        GameObject gem = Instantiate(gems[gemIndex], gemPosition.position, Quaternion.identity);
        gem.transform.localScale = Vector3.one * 3;

        audioSource.clip = dyingClip;
        audioSource.PlayOneShot(dyingClip);

        Destroy(trollParent);
    }
}


/*Old troll

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


*/

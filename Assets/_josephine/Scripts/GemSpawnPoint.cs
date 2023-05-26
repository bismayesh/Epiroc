using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnPoint : MonoBehaviour
{
    public TaskDrill taskDrill;

    public List<GameObject> gemObjects = new List<GameObject>();

    private void Start()
    {
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            taskDrill.SpawnArea = true;
            taskDrill.GemSpawnObject = gameObject.GetComponent<GemSpawnPoint>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            taskDrill.SpawnArea = false;
            taskDrill.GemSpawnObject = null;
        }
    }

    public void SpawnGems()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var g in gemObjects)
            {
                Vector3 randomDir = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(2.0f, 5.0f), Random.Range(-3.0f, 3.0f));
                Instantiate(g);
                Rigidbody rigidbody = g.GetComponent<Rigidbody>();
                rigidbody.AddForce(randomDir);
            }
        }

        Destroy(gameObject, 2f);
    }
}

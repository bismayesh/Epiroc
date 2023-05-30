using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnPoint : MonoBehaviour
{
    public TaskDrill taskDrill;
    public List<GameObject> gemObjects = new List<GameObject>();
    public bool spawnGems = false;
    public Transform spawnTransform;
    Vector3 spawnPosition;

    private void Start()
    {
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
        spawnPosition = spawnTransform.position;
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

    private void Update()
    {
        if (spawnGems)
        {
            spawnGems = false;
            StartCoroutine(SpawnDelay());
        }
    }

    public void SpawnGems()
    {
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (var g in gemObjects)
            {
                Vector3 randomDir = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(5.0f, 15.0f), Random.Range(-5.0f, 5.0f));
                var gem = Instantiate(g, spawnPosition, Quaternion.identity);
                gem.GetComponent<Rigidbody>().AddForce(randomDir, ForceMode.Impulse);

                
                yield return new WaitForSeconds(0.2f);
            }
        }

        Destroy(gameObject, 2.5f);
    }
}

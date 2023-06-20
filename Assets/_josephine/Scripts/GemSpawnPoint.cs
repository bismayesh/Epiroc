using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnPoint : MonoBehaviour
{
    public GameObject trollPrefab;
    public List<GameObject> stones = new List<GameObject>();
    public int spawnCount;
    public AudioSource audioSource;
    public AudioClip explosionClip;
    public bool spawn = false;
    public Transform stoneSpawnTransform;
    public Transform trollSpawnTransform;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            TaskDrill.instance.SpawnArea = true;
            TaskDrill.instance.GemSpawnObject = gameObject.GetComponent<GemSpawnPoint>();
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            TaskDrill.instance.SpawnArea = false;
            TaskDrill.instance.GemSpawnObject = null;
            audioSource.Stop();
        }
    }

    private void Update()
    {
        if (spawn)
        {
            spawn = false;
            StartCoroutine(SpawnDelay());
        }
    }

    public void GemPilesSpawn()
    {
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        audioSource.clip = explosionClip;
        audioSource.PlayOneShot(explosionClip);

        StartCoroutine (SpawnStones());

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(trollPrefab, trollSpawnTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }

        TrollKillTorch.instance.EmptyTrollsList();
        Destroy(gameObject);
    }

    IEnumerator SpawnStones()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            foreach (var s in stones)
            {
                Vector3 randomDir = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.5f, 3.0f), Random.Range(-2.0f, 2.0f));
                var stone = Instantiate(s, stoneSpawnTransform.position, Quaternion.identity);
                float size = Random.Range(2, 6);
                stone.transform.localScale = new Vector3(size, size, size);
                stone.GetComponent<Rigidbody>().AddForce(randomDir, ForceMode.Impulse);

                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSpawner : MonoBehaviour
{
    public GameObject trollPrefab;
    public Transform[] spawnPoints;
    public int spawnCount = 3;
    int index;

    public static TrollSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InstanciateTrolls();
    }

    public void InstanciateTrolls()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            index = ChooseSpawnPoint();
            Instantiate(trollPrefab, spawnPoints[index].position, Quaternion.identity);
        }
    }

    int ChooseSpawnPoint()
    {
        return Random.Range(0, spawnPoints.Length - 1);
    }
}

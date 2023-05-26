using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveWalls : MonoBehaviour
{
    public TrainingState currentTraining;

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            currentTraining.MachineDamage++;
        }
    }
}

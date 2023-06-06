using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveWalls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            TrainingState.instance.MachineDamage(1);
        }

        if (other.gameObject.CompareTag("ChunkGem"))
        {
            Destroy(other.gameObject);
        }
    }
}

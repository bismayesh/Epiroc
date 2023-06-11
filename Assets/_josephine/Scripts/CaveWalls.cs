using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveWalls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            TrainingState.instance.MachineDamage(5);
        }

        /*
        if (other.gameObject.CompareTag("Stone"))
        {
            Destroy(other.gameObject);
        }
        */
        
    }
}

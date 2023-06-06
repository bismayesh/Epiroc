using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cave"))
        {
            TrainingState.instance.MachineDamage(5);
        }
    }
}

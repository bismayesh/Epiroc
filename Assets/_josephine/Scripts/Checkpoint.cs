using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            TrainingState.instance.UpdateScoreMultiplier();
            TaskDrive.instance.DriveProgress(gameObject);
        }
    }
}

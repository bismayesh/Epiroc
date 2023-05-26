using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public TaskDrive taskDrive;

    private void Start()
    {
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machine"))
        {
            taskDrive.CurrentCheckpoint++;
        }
    }
}

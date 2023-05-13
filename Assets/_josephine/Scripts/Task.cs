using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string taskName;
    public List<GameObject> subTasks = new List<GameObject>();
    public Training currentTraining;

    int index = 0;

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<Training>();
    }

    public void ButtonController(GameObject pressedButton)
    {
        if (subTasks[index] == pressedButton)
        {
            //Öka progress
            Debug.Log("Task " + taskName + " progress");
            index++;

            if (index == subTasks.Count)
            {
                currentTraining.TaskIndex++;
                Debug.Log("Task " + taskName + " finnished");
            }
        }
        else
        {
            TaskFailure();
            Debug.Log("Task " + taskName + " faild");
        }
    }

    void TaskFailure()
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour
{
    public string trainingName;
    public List<Task> tasks = new List<Task>();

    GameObject currentTrainingTask;
    int index = 0;

    public int TaskIndex
    {
        get { return index; }
        set
        {
            index = value;


            if (index >= tasks.Count)
            {
                //Whole training finnished
            }
            else
            {
                //Progress increase condition
            }
        }
    }

    public GameObject CurrentTask
    {
        get { return currentTrainingTask; }
        private set
        {
            currentTrainingTask = value;
        }
    }

    private void Update()
    {
        currentTrainingTask = tasks[index].gameObject;
    }

    public void ButtonProgressTracking()
    {
        tasks[index].ButtonController(gameObject);
    }
}
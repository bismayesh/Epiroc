using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public string taskName;
    public List<GameObject> subTasks = new List<GameObject>();
    public Training currentTraining;
    public GameState gameManager;

    int index = 0;
    int progress = 0;

    //event
    public UnityEvent OnTaskFinished;
    //public UnityEvent OnTaskFinished<Vecto2 joystick>;

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<Training>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameState>();
    }



    public void ButtonController(GameObject pressedButton)
    {
        if (subTasks[index] == pressedButton)
        {
            //�ka progress
            index++;
            progress = (int)((float)index / (float)subTasks.Count * 100);
            gameManager.TaskProgress = progress;

            if (index == subTasks.Count)
            {
                currentTraining.TaskIndex++;
                Debug.Log("Task " + taskName + " finnished");
                OnTaskFinished?.Invoke();
            }
        }
        else
        {
            TaskFailure();
        }
    }

    void TaskFailure()
    {
        index = 0;
        progress = 0;
        gameManager.TaskProgress = progress;
        gameManager.UpdateTrainingFailures();
        gameManager.UpdateTrainingAttempts();
    }
}
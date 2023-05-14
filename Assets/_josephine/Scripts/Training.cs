using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour
{
    public string trainingName;
    public List<Task> tasks = new List<Task>();
    public GameState gameManager;

    GameObject currentTrainingTask;
    int index = 0;
    int trainingProgress = 0;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameState>();
        gameManager.TrainingName = trainingName;
        gameManager.TaskName = tasks[0].taskName;
        currentTrainingTask = tasks[0].gameObject;
    }

    public int TaskIndex
    {
        get { return index; }
        set
        {
            index = value;

            if(index < tasks.Count)
            {
                currentTrainingTask = tasks[index].gameObject;
            }
            
            if (index == tasks.Count)
            {
                //Whole training finnished
                Debug.Log("Training finnished!");
                gameManager.TrainingProgress = 100;
            }
            else
            {
                StartCoroutine(TaskProgress());
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

    /*
    public void ButtonProgressTracking()
    {
        tasks[index].ButtonController(gameObject);
    }
    */

    IEnumerator TaskProgress()
    {
        yield return new WaitForSeconds(2);
        trainingProgress = (int)((float)index / (float)tasks.Count * 100);
        gameManager.TrainingProgress = trainingProgress;
        gameManager.TaskProgress = 0;
        gameManager.TaskName = tasks[index].taskName;
    }
}
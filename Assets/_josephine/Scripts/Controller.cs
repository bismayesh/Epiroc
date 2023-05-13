using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Training activeTraining;
    public Task activeTask;

    private void Start()
    {
        activeTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<Training>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player pressed button");
        }
    }

    public void ButtonFunction()
    {
        activeTask = activeTraining.CurrentTask.GetComponent<Task>();
        activeTask.ButtonController(gameObject);
    }
}

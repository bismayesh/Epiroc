using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Training activeTraining;
    public Task activeTask;

    //bool pressing = false;

    private void Start()
    {
        activeTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<Training>();
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Player" && !pressing)
        {
            pressing = true;
            Debug.Log("Player pressed button");
            activeTask = activeTraining.CurrentTask.GetComponent<Task>();
            activeTask.ButtonController(gameObject);

            StartCoroutine(WaitTwoSeconds());
        }
    }

    private IEnumerator WaitTwoSeconds()
    {
        yield return new WaitForSeconds(2);
        pressing = false;
    }
    */

    
    public void ButtonFunction()
    {
        activeTask = activeTraining.CurrentTask.GetComponent<Task>();
        activeTask.ButtonController(gameObject);
    }
    
}

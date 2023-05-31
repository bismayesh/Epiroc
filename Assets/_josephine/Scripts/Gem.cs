using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemScore;
    [HideInInspector]
    public TrainingState trainingState;

    private void Start()
    {
        trainingState = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Machine"))
        {
            trainingState.UpdateTrainingScore(gemScore);

            Destroy(gameObject);
        }
    }
}

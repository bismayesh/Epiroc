using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemScore;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Machine"))
        {
            TrainingState.instance.UpdateTrainingScore(gemScore);
            Destroy(gameObject);
        }

        /*
        if (collision.gameObject.CompareTag("Troll"))
        {
            Destroy(gameObject);
        }
        */
    }
}

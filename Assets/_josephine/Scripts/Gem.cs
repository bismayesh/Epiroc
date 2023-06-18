using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemScore;
    public bool superGem = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Machine"))
        {
            TrainingState.instance.UpdateTrainingScore(gemScore);
            Destroy(gameObject);

            if (superGem)
            {
                TrainingState.instance.TrainingFinnished();
            }
        }
    }
}

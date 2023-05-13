using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    [SerializeField]
    private int playerHP = 100;
    private int playerProgress = 0;

    public int HP
    {
        get { return playerHP; }
        set
        {
            //Change value example:
            //private GameBehavior gameManager;
            //gameManager.HP -= 1;

            playerHP = value;
            Debug.LogFormat("HP: {0}", playerHP);

            if (playerHP <= 0)
            {
                //Player died condition
            }
            else
            {
                //Hurt condition
            }
        }
    }

    public int Progress
    {
        get { return playerProgress; }
        set
        {
            playerProgress = value;
            Debug.LogFormat("Prrogress: {0}%", playerProgress);

            if (playerProgress >= 100)
            {
                //Finnished training condition
            }
            else
            {
                //Progress increase condition
            }
        }
    }
}

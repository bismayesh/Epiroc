using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    void TransitionToMainScene()
    {
        SceneManager.LoadScene(1);
    }
}

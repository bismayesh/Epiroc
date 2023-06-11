using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroScene : MonoBehaviour
{
    public void TransitionToMainScene()
    {
        SceneManager.LoadScene(1);
    }
}

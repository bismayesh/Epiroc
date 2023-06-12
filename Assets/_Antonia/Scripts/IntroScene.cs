using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class IntroScene : MonoBehaviour
{
    public InputActionProperty primaryButtonAction;
    float primaryButtonValue;
    public void FixedUpdate()
    {
        primaryButtonValue = primaryButtonAction.action.ReadValue<float>();
        if (primaryButtonValue != 0.0f)
        {
            TransitionToMainScene();
        }
    }
    public void TransitionToMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class IntroScene : MonoBehaviour
{
    public InputActionProperty primaryButtonAction;
    public AudioClip introClip;
    float primaryButtonValue;

    private void Start()
    {
        StartCoroutine(ForcedSceneTransition());
    }

    IEnumerator ForcedSceneTransition()
    {
        yield return new WaitForSeconds(introClip.length);
        SceneManager.LoadScene(1);
    }

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
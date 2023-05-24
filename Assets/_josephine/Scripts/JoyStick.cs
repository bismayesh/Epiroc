using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoyStick : MonoBehaviour
{
    public InputActionProperty primaryButtonAction;
    public InputActionProperty secondaryButtonAction;

    public Renderer light1;
    public Renderer light2;
    public Material MaterialOn;
    public Material MaterialOff;

    public bool primaryIsActive = false;
    public bool secondaryIsActive = false;
    float primaryButton;
    float secondaryButton;
    public bool buttonJustPressed  = false;

    void FixedUpdate()
    {
        primaryButton = primaryButtonAction.action.ReadValue<float>();
        secondaryButton = secondaryButtonAction.action.ReadValue<float>();

        //Debug.Log(secondaryButton);
    }
    private void OnTriggerStay(Collider other)
    {
        TopButtonPressed(other, primaryButton, ref primaryIsActive, light1);
        TopButtonPressed(other, secondaryButton, ref secondaryIsActive, light2);
    }

    private void TopButtonPressed(Collider other, float topButton, ref bool isActive, Renderer light)
    {
        if (other.gameObject.CompareTag("Player") && topButton != 0f && !buttonJustPressed)
        {
            buttonJustPressed = true;
            StartCoroutine(ButtonPressDelay());

            if (isActive)
            {
                isActive = false;
                light.material = MaterialOff;
            }
            else
            {
                isActive = true;
                light.material = MaterialOn;
            }
        }
    }

    IEnumerator ButtonPressDelay()
    {
        yield return new WaitForSeconds(0.1f);
        buttonJustPressed = false;
    }
}

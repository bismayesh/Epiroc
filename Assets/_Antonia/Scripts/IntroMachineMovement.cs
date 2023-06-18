using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMachineMovement : MonoBehaviour
{
    [HideInInspector]
    public MachineController controller;
    void Start()
    {
        controller = FindObjectOfType<MachineController>();
        StartCoroutine(MachineMovement());
    }

    IEnumerator MachineMovement()
    {
        controller.ExtendJacks();
        yield return new WaitForSeconds(5);

        controller.RetrieveJacks();
        yield return new WaitForSeconds(5);

        StartCoroutine(MachineMovement());
    }
}

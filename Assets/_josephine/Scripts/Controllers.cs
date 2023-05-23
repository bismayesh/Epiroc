using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public Transform knobAngle;
    public TaskStart startMachine;
    public bool isActive = false;

    private float localRotation;

    private void Start()
    {
        startMachine = GameObject.Find("StartMachine").GetComponent<TaskStart>();
    }

    public void ButtonPressed()
    {
        startMachine.ActivateButton(this.gameObject);
    }

    public void SwitchTurned()
    {
        localRotation = knobAngle.localRotation.eulerAngles.y;
        localRotation = (Mathf.Abs(localRotation))%180;

        if (localRotation == 0)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }
        startMachine.StartEngine(this.gameObject, isActive);
    }
}

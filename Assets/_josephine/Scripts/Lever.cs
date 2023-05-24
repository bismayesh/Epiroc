using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lever : MonoBehaviour
{
    [SerializeField]
    Transform hingeTransform;
    [SerializeField]
    Renderer lightOn;
    [SerializeField]
    Renderer lightOff;
    [SerializeField]
    Material materialOn;
    [SerializeField]
    Material materialOff;
    [SerializeField]
    Material materialNoLight;

    private bool isActive = false;

    public void SwitchTurning()
    {
        float angle = hingeTransform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        if (angle >= 60)
        {
            isActive = true;
            lightOn.material = materialOn;
            lightOff.material = materialNoLight;
        }
        else if (angle < 60 && angle >= -60)
        {
            lightOn.material = materialNoLight;
            lightOff.material = materialNoLight;
        }
        else
        {
            isActive = false;
            lightOn.material = materialNoLight;
            lightOff.material = materialOff;
        }
    }

    public void SwitchOnOff()
    {
        //Debug.Log("Switch turned!");
        //taskDrive.StartEngine(this.gameObject, isActive);
    }
}

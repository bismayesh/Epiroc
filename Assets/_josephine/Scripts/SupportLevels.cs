using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportLevels : MonoBehaviour
{
    enum SupportLayers { Light, Text, Voice, Ghost}

    //Intro
    public GameObject introText;

    //Drive
    public GameObject driveTask1;
    public GameObject driveTask2;
    public GameObject driveTask3;

    public GameObject driveText;

    public GameObject driveLight1;
    public GameObject driveLight2;
    public GameObject driveLight3;

    bool supportStarted = false;

    //Drill


    //Torch

    private void Start()
    {
        introText.SetActive(true);
        StartCoroutine(DriveTaskDelay());
    }

    public void UserSupport(GameObject thisObject)
    {
        if (!supportStarted)
        {
            supportStarted = true;
            driveText.SetActive(true);
            driveLight1.SetActive(true);
        }
        else if (thisObject == driveTask1)
        {
            driveLight1.SetActive(false);
            driveLight2.SetActive(true);
        }
        else if (thisObject == driveTask2)
        {
            driveLight2.SetActive(false);
            driveLight3.SetActive(true);
        }
        else if (thisObject == driveTask3)
        {
            driveLight3.SetActive(false);
            driveText.SetActive(false);
        }
    }

    IEnumerator DriveTaskDelay()
    {
        yield return new WaitForSeconds(10);
        introText.SetActive(false);
        UserSupport(gameObject);
    }
}

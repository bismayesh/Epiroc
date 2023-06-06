using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SupportLevels : MonoBehaviour
{
    //Support menu
    [Header("Support Menu")]
    public GameObject supportMenu;
    public InputActionReference InputAction = default;
    public bool holdingJoystick = false;
    public Toggle supportTextToggle;
    public Toggle supportLightToggle;
    public Toggle supportVoiceToggle;
    public Toggle supportGhostToggle;

    //Intro
    [Header("Intro")]
    public GameObject textBackground;
    public GameObject introText;
    public float introTime = 10.0f;

    //Drive
    [Header("Drive")]
    public GameObject driveTask1;
    public GameObject driveTask2;
    public GameObject driveTask3;

    public GameObject driveText;

    public GameObject driveLight1;
    public GameObject driveLight2;
    public GameObject driveLight3;

    //Drill
    [Header("Drill")]
    public GameObject drillTask1;
    public TextMeshProUGUI textFrontJacks;
    public TextMeshProUGUI textRearJacks;
    public GameObject drillTask3;

    public GameObject drillText;

    public GameObject drillLight1;
    public GameObject drillLight2;
    public GameObject drillLight3;

    string frontJacks;
    string rearJacks;
    [SerializeField]
    bool taskJacksComplete = false;
    [SerializeField]
    bool taskHasRun = false;
    bool firstTrollSpawn = true;

    //Torch
    [Header("Torch")]
    public GameObject torchTask1;
    public GameObject torchTask2;
    public GameObject torchTask3;

    public GameObject torchText;

    public GameObject torchLight1;
    public GameObject torchLight2;
    public GameObject torchLight3;

    //Supportlayers
    enum SupportLayers { Light, Text, Voice, Ghost }
    private SupportLayers supportLight = SupportLayers.Light;
    [SerializeField]
    bool supportlayerText = true;
    [SerializeField]
    bool supportlayerLight = true;
    [SerializeField]
    bool supportlayerVoice = true;
    [SerializeField]
    bool supportlayerGhost = true;

    [SerializeField]
    bool driveSupportOn = true;
    [SerializeField]
    bool drillSupportOn = false;
    [SerializeField]
    bool torchSupportOn = false;
    [SerializeField]
    bool supportStarted = false;
    bool supportTaskRunning = false;

    //Singelton
    public static SupportLevels instance;

    public bool DriveSupportOn
    {
        get { return driveSupportOn; }
        set { driveSupportOn = value; }
    }

    public bool DrillSupportOn
    {
        get { return drillSupportOn; }
        set { drillSupportOn = value; }
    }

    public bool TorchSupportOn
    {
        get { return torchSupportOn; }
        set { torchSupportOn = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        textBackground.SetActive(true);
        introText.SetActive(true);
        StartCoroutine(DriveTaskDelay());
    }

    IEnumerator DriveTaskDelay()
    {
        yield return new WaitForSeconds(introTime);
        introText.SetActive(false);
        textBackground.SetActive(false);
        driveSupportOn = true;
        UserSupport(gameObject);
    }

    private void FixedUpdate()
    {
        frontJacks = textFrontJacks.text;
        rearJacks = textRearJacks.text;

        if (drillSupportOn && frontJacks == "Front Jacks Up" && rearJacks == "Rear Jacks Up" && !taskJacksComplete)
        {
            taskJacksComplete = true;
            DrillSupport(gameObject);
        }
    }

    public void UserSupport(GameObject thisObject)
    {
        if (driveSupportOn)
        {
            DriveSupport(thisObject);
        }
        else if (drillSupportOn)
        {
            DrillSupport(thisObject);
        }
        else if (torchSupportOn)
        {
            TorchSupport(thisObject);
        } 
    }

    public void DriveInstructions()
    {
        DriveSupportOn = true;
        DriveSupport(gameObject);
    }

    public void DrillInstructions()
    {
        DrillSupportOn = true;
        DrillSupport(gameObject);
    }

    public void TorchInstructions()
    {
        TorchSupportOn = true;
        TorchSupport(gameObject);
    }

    private void DriveSupport(GameObject thisObject)
    {
        supportTaskRunning = true;

        if (!supportStarted)
        {
            supportStarted = true;
            if (supportlayerText)
            {
                textBackground.SetActive(true);
                driveText.SetActive(true);
            }
            if (supportlayerLight)
            {
                driveLight1.SetActive(true);
            }
        }
        else if (thisObject == driveTask1)
        {
            if (supportlayerLight)
            {
                driveLight1.SetActive(false);
                driveLight2.SetActive(true);
            }
            
        }
        else if (thisObject == driveTask2)
        {
            if (supportlayerLight)
            {
                driveLight2.SetActive(false);
                driveLight3.SetActive(true);
            }
            
        }
        else if (thisObject == driveTask3)
        {
            if (supportlayerText)
            {
                textBackground.SetActive(false);
                driveText.SetActive(false);
            }
            if (supportlayerLight)
            {
                driveLight3.SetActive(false);
            }
            
            supportStarted = false;
            driveSupportOn = false;
            supportTaskRunning = false;
        }
    }

    private void DrillSupport(GameObject thisObject)
    {
        supportTaskRunning = true;

        if (!supportStarted)
        {
            supportStarted = true;
            if (supportlayerText)
            {
                textBackground.SetActive(true);
                drillText.SetActive(true);
            }
            if (supportlayerLight)
            {
                drillLight1.SetActive(true);
            }
        }
        else if (thisObject == drillTask1)
        {
            if (supportlayerLight)
            {
                drillLight1.SetActive(false);
                drillLight2.SetActive(true);
            }

        }
        else if (taskJacksComplete && !taskHasRun)
        {
            taskHasRun = true;
            if (supportlayerLight)
            {
                drillLight2.SetActive(false);
                drillLight3.SetActive(true);
            }
        }
        else if (thisObject == drillTask3)
        {
            if (supportlayerText)
            {
                drillText.SetActive(false);
                textBackground.SetActive(false);
            }
            if (supportlayerLight)
            {
                drillLight3.SetActive(false);
            }

            supportStarted = false;
            drillSupportOn = false;
            taskJacksComplete = false;
            taskHasRun = false;
            supportTaskRunning = false;
        }
    }

    private void TorchSupport(GameObject thisObject)
    {
        supportTaskRunning = true;

        if (!supportStarted)
        {
            supportStarted = true;
            if (supportlayerText)
            {
                textBackground.SetActive(true);
                torchText.SetActive(true);
            }
            if (supportlayerLight)
            {
                torchLight1.SetActive(true);
            }
        }
        else if (thisObject == torchTask1)
        {
            if (supportlayerLight)
            {
                torchLight1.SetActive(false);
                torchLight2.SetActive(true);
            }

        }
        else if (thisObject == torchTask2)
        {
            if (supportlayerLight)
            {
                torchLight2.SetActive(false);
                torchLight3.SetActive(true);
            }

        }
        else if (thisObject == torchTask3)
        {
            if (supportlayerText)
            {
                torchText.SetActive(false);
                textBackground.SetActive(false);
            }
            if (supportlayerLight)
            {
                torchLight3.SetActive(false);
            }

            supportStarted = false;
            torchSupportOn = false;
            supportTaskRunning = false;

            if (firstTrollSpawn)
            {
                firstTrollSpawn = false;
                TrollSpawner.instance.InstanciateTrolls();
            }
        }
    }


    private void OnEnable()
    {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable()
    {
        InputAction.action.performed -= ToggleActive;
    }

    public void ToggleActive(InputAction.CallbackContext context)
    {
        if (supportMenu && !holdingJoystick)
        {
            supportMenu.SetActive(!supportMenu.activeSelf);
        }
    }

    public void TextSupportButton()
    {
        CheckSupportToggle( supportTextToggle,ref supportlayerText);
    }

    public void LightSupportButton()
    {
        CheckSupportToggle(supportLightToggle, ref supportlayerLight);
    }

    public void VoiceSupportButton()
    {
        CheckSupportToggle(supportVoiceToggle, ref supportlayerVoice);
    }

    public void GhostSupportButton()
    {
        CheckSupportToggle(supportGhostToggle, ref supportlayerGhost);
    }

    private void CheckSupportToggle(Toggle toggle,ref bool supportLayer)
    {
        toggle.isOn = !toggle.isOn;
        supportLayer = toggle.isOn;
    }
}



//Testa och skriva om klassen

/*
public enum SupportLayer
{
    Text,
    Light,
    Voice,
    Ghost
}


[System.Serializable]
public class SupportSystem
{
    public List<SupportLayer> layers;
} 

public class Tasks
{
    public List<GameObject> tasks = new List<GameObject>();
    public List<TextMeshProUGUI> textTasks = new List<TextMeshProUGUI>();
    public List<string> taskMessages = new List<string>();

    bool textTaskComplete = false;

    private void FixedUpdate()
    {
        if (textTasks[0].text == taskMessages[0] && textTasks[1].text == taskMessages[1] && !textTaskComplete)
        {
            textTaskComplete = true;
            TaskCompleted(textTaskComplete);
        }
    }

    public bool TaskCompleted(GameObject taskObject, GameObject thisObject)
    {
        if (taskObject = thisObject)
        {
            return true;
        }
        return false;
    }

    public bool TaskCompleted(bool textTaskComplete)
    {
        if (textTaskComplete)
        {
            return true;
        }
        return false;
    }
}
*/


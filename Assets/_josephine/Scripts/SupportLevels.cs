using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum SupportMood
{
    None,
    Old,
    Intro,
    Drive,
    Drill,
    Torch
}

public class SupportLevels : MonoBehaviour
{
    //Support menu
    [Header("Support Menu")]
    public InputActionReference InputAction = default;
    public GameObject supportMenu;
    public Toggle supportTextToggle;
    public Toggle supportLightToggle;
    public Toggle supportVoiceToggle;
    public Toggle supportGhostToggle;
    bool holdingJoystick = false;

    //Intro
    [Header("Intro")]
    public GameObject textBackground;
    public GameObject introText;
    public float introTime = 10.0f;

    //Tasks
    List<SingleTask> driveTasks;
    List<SingleTask> drillTasks;
    List <SingleTask> torchTasks;
    public SupportMood supportMood;
    [SerializeField]
    int index = 0;

    //Supportlayers
    [SerializeField]
    bool supportlayerText = true;
    [SerializeField]
    bool supportlayerLight = true;
    [SerializeField]
    bool supportlayerVoice = true;
    [SerializeField]
    bool supportlayerGhost = true;

    //Singelton
    public static SupportLevels instance;

    public bool HoldingJoystick
    {
        get { return holdingJoystick; }
        set { holdingJoystick = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        driveTasks = TaskDrive.instance.taskControl;
        drillTasks = TaskDrill.instance.taskControl;
        torchTasks = TaskTorch.instance.taskControl;

        SupportInstructions(SupportMood.Intro);
    }

    private void FixedUpdate()
    {
        if (supportMood == SupportMood.Drill && TaskDrill.instance.frontJacksUp && TaskDrill.instance.rearJacksUp)
        {
            SupportLayerChain(drillTasks);
        }
    }

    public void SupportInstructions(SupportMood newMood = SupportMood.Old, GameObject thisObject = null, bool resetIndex = false)
    {
        if (newMood != SupportMood.Old)
            supportMood = newMood;

        if (resetIndex)
            index = 0;

        switch (supportMood)
        {
            case SupportMood.None: break;
            case SupportMood.Intro: IntroSupport(); break;
            case SupportMood.Drive: SupportLayerChain(driveTasks, thisObject); break;
            case SupportMood.Drill: SupportLayerChain(drillTasks, thisObject); break;
            case SupportMood.Torch: SupportLayerChain(torchTasks, thisObject); break;
        } 
    }

    void IntroSupport()
    {
        textBackground.SetActive(true);
        introText.SetActive(true);
        StartCoroutine(IntroTaskTime());
    }

    IEnumerator IntroTaskTime()
    {
        yield return new WaitForSeconds(introTime);
        introText.SetActive(false);
        textBackground.SetActive(false);

        SupportInstructions(SupportMood.Drive);
    }

    void SupportLayerChain(List<SingleTask> tasks, GameObject thisObject = null)
    {
        if (index == 0 || tasks[index - 1].task == thisObject)
        {
            if (index == tasks.Count)
            {
                index = 0;
                supportMood = SupportMood.None;
                ResetSupportLayers();
                return;
            }


            if (supportlayerText)
            {
                textBackground.SetActive(true);
                if (index != 0) tasks[index - 1].supportText.SetActive(false);
                tasks[index].supportText.SetActive(true);
            }
            if (supportlayerLight)
            {
                if (index != 0) tasks[index - 1].supportLight.SetActive(false);
                tasks[index].supportLight.SetActive(true);
            }
            /*
            if (supportlayerVoice)
            {
                if (index != 0)
                    tasks[index - 1].supportVoice.SetActive(false);
                tasks[index].supportVoice.SetActive(true);
            }
            if (supportlayerGhost)
            {
                if (index != 0)
                    tasks[index - 1].supportGhost.SetActive(false);
                tasks[index].supportGhost.SetActive(true);
            }*/

            index++;
        }
    }

    void ResetSupportLayers()
    {
        textBackground.SetActive(false);
        introText.SetActive(false);

        foreach (SingleTask task in driveTasks)
        {
            task.supportText.SetActive(false);
            task.supportLight.SetActive(false);
            //task.supportVoice.SetActive(false);
            //task.supportGhost.SetActive(false);
        }

        foreach (SingleTask task in drillTasks)
        {
            task.supportText.SetActive(false);
            task.supportLight.SetActive(false);
            //task.supportVoice.SetActive(false);
            //task.supportGhost.SetActive(false);
        }

        foreach (SingleTask task in torchTasks)
        {
            task.supportText.SetActive(false);
            task.supportLight.SetActive(false);
            //task.supportVoice.SetActive(false);
            //task.supportGhost.SetActive(false);
        }
    }

    //Buttons

    public void ButtonDrive()
    {
        ResetSupportLayers();
        SupportInstructions(SupportMood.Drive, default, true);
    }

    public void ButtonDrill()
    {
        ResetSupportLayers();
        SupportInstructions(SupportMood.Drill, default, true);
    }

    public void ButtonTorch()
    {
        ResetSupportLayers();
        SupportInstructions(SupportMood.Torch, default, true);
    }

    public void ButtonTextSupport()
    {
        ResetSupportLayers();
        CheckSupportToggle(supportTextToggle,ref supportlayerText);
    }

    public void ButtonLightSupport()
    {
        ResetSupportLayers();
        CheckSupportToggle(supportLightToggle, ref supportlayerLight);
    }

    public void ButtonVoiceSupport()
    {
        ResetSupportLayers();
        CheckSupportToggle(supportVoiceToggle, ref supportlayerVoice);
    }

    public void ButtonGhostSupport()
    {
        ResetSupportLayers();
        CheckSupportToggle(supportGhostToggle, ref supportlayerGhost);
    }

    private void CheckSupportToggle(Toggle toggle,ref bool supportLayer)
    {
        toggle.isOn = !toggle.isOn;
        supportLayer = toggle.isOn;
    }

    //Turn hand menu on/off

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
        if (supportMenu.activeSelf || !holdingJoystick)
        {
            supportMenu.SetActive(!supportMenu.activeSelf);
        }
    }





    /*
     * 
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

    [SerializeField]
    bool driveSupportOn = true;
    [SerializeField]
    bool drillSupportOn = false;
    [SerializeField]
    bool torchSupportOn = false;
    [SerializeField]
    bool supportStarted = false;
    bool supportTaskRunning = false;

     * 
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
     
    






    private void DriveSupportOld(GameObject thisObject)
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
     */
}





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
    List<SingleTask> torchTasks;
    public SupportMood supportMood;
    [SerializeField]
    int index = 0;
    bool firstTorchInstruction = true;

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
                if (supportMood == SupportMood.Torch && firstTorchInstruction)
                {
                    firstTorchInstruction = false;
                    TrollSpawner.instance.InstanciateTrolls();
                }

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
        CheckSupportToggle(supportTextToggle, ref supportlayerText);
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

    private void CheckSupportToggle(Toggle toggle, ref bool supportLayer)
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
}





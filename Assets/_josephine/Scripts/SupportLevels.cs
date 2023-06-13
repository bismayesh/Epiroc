using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Voice;

public enum SupportMood
{
    None,
    Old,
    Introduction,
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

    public GameObject supportAnimatedControlers;
    bool animatedContolersIsOn = false;
    public GameObject objectivesCanvas;
    public AudioSource objectiveAudioSource;
    public AudioClip objectiveClip;
    bool objectiveCanvasIsOn = false;
    public GameObject dontForgetParkBrake;
    public GameObject endDriveGhost;
    Coroutine objectiveRoutine;

    //Intro
    [Header("Intro")]
    public GameObject textBackground;
    public GameObject introText;
    public float introTime = 10.0f;
    public AudioSource taskCompleteSource;
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip introClip;
    public AudioClip taskCompleteClip;
    public AudioClip driveFinalClip;
    public AudioClip drillFinalClip;
    public AudioClip torchFinalClip;
    [SerializeField]
    bool introFinnished = false;
    [SerializeField]
    public bool instructionsFinnished = false;
    bool xButtonPressed = false;
    Coroutine lastCoroutine = null;

    //Tasks
    List<SingleTask> instructionTasks;
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
        audioSource = GetComponent<AudioSource>();
        taskCompleteSource.clip = taskCompleteClip;
        instructionTasks = TaskInstructions.instance.taskControl;
        driveTasks = TaskDrive.instance.taskControl;
        drillTasks = TaskDrill.instance.taskControl;
        torchTasks = TaskTorch.instance.taskControl;

        supportAnimatedControlers.SetActive(false);
        supportMenu.SetActive(false);

        ResetSupportLayers();
        StartCoroutine(IntroSupport());
    }

    public void SupportInstructions(SupportMood newMood = SupportMood.Old, GameObject thisObject = null, bool resetIndex = false)
    {
        if (!introFinnished)
            return;

        if (newMood != SupportMood.Old)
            supportMood = newMood;

        if (resetIndex)
            index = 0;

        switch (supportMood)
        {
            case SupportMood.None: ResetSupportLayers(); break;
            case SupportMood.Introduction: SupportLayerChain(instructionTasks, thisObject); break;
            case SupportMood.Drive: SupportLayerChain(driveTasks, thisObject); break;
            case SupportMood.Drill: SupportLayerChain(drillTasks, thisObject); break;
            case SupportMood.Torch: SupportLayerChain(torchTasks, thisObject); break;
        }
    }

    IEnumerator IntroSupport()
    {
        textBackground.SetActive(true);
        introText.SetActive(true);
        StartCoroutine(PlayAudio(introClip));

        yield return new WaitForSeconds(introClip.length);

        introText.SetActive(false);
        textBackground.SetActive(false);
        introFinnished = true;

        supportMenu.SetActive(true);
        SupportInstructions(SupportMood.Introduction);
    }

    IEnumerator PlayAudio(AudioClip clip)
    {
        if(lastCoroutine != null)
            StopCoroutine(lastCoroutine);

        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        //audioSource.Stop();
        //audioSource.clip = null;
    }

    void SupportLayerChain(List<SingleTask> tasks, GameObject thisObject = null)
    {
        if (index == 0 || tasks[index - 1].IsOn && tasks[index - 1].task == thisObject || !instructionsFinnished && index == 2 || !instructionsFinnished && index == 4 && xButtonPressed || !instructionsFinnished && index == 5 || !instructionsFinnished && index == 6)
        {
            

            if (index != 0)
            {
                Debug.Log(tasks[index - 1].task + " ska va samma som " + thisObject);
                audioSource.PlayOneShot(taskCompleteClip, 0.2f);
            }

            if (index == tasks.Count )
            {
                if (supportMood == SupportMood.Torch)
                {
                    taskCompleteSource.PlayOneShot(taskCompleteClip, 0.2f);
                    lastCoroutine = StartCoroutine(PlayAudio(torchFinalClip));

                    /*
                    if (firstTorchInstruction)
                    {
                        firstTorchInstruction = false;
                        TrollSpawner.instance.InstanciateTrolls();
                    }
                    */
                }

                if (supportMood == SupportMood.Drive)
                {
                    taskCompleteSource.PlayOneShot(taskCompleteClip, 0.2f);
                    lastCoroutine = StartCoroutine(PlayAudio(driveFinalClip));
                    if (supportlayerGhost)
                        StartCoroutine(ShowObjectTimer(endDriveGhost, 5.0f));
                }

                if (supportMood == SupportMood.Drill)
                {
                    taskCompleteSource.PlayOneShot(taskCompleteClip, 0.2f);
                    lastCoroutine = StartCoroutine(PlayAudio(drillFinalClip));
                }

                if (supportMood == SupportMood.Introduction)
                {
                    taskCompleteSource.PlayOneShot(taskCompleteClip, 0.2f);
                    instructionsFinnished = true;
                }

                index = 0;
                supportMood = SupportMood.None;
                ResetSupportLayers();
                return;
            }

            if (index == tasks.Count + 1 && supportMood == SupportMood.Introduction)
            {
                instructionsFinnished = true;
                index = 0;
                supportMood = SupportMood.None;
                ResetSupportLayers();
                return;
            }

            if (supportlayerText)
            {
                textBackground.SetActive(true);
                if (index != 0)
                {
                    tasks[index - 1].supportText.SetActive(false);
                    tasks[index - 1].supportTextSmall.SetActive(false);
                }
                tasks[index].supportText.SetActive(true);
                tasks[index].supportTextSmall.SetActive(true);

                if (supportMood == SupportMood.Drill && index == 0)
                    StartCoroutine(ShowObjectTimer(dontForgetParkBrake, 3.0f, 1.5f));
            }
            if (supportlayerLight)
            {
                if (index != 0) tasks[index - 1].supportLight.SetActive(false);
                tasks[index].supportLight.SetActive(true);
            }
            if (supportlayerVoice)
            {
                lastCoroutine = StartCoroutine(PlayAudio(tasks[index].supportVoice));
            }
            if (supportlayerGhost)
            {
                if (index != 0)
                    tasks[index - 1].supportGhost.SetActive(false);
                tasks[index].supportGhost.SetActive(true);
            }

            index++;


            //Instructions
            if (!instructionsFinnished && index == 2 || !instructionsFinnished && index == 5 || !instructionsFinnished && index == 6)
            {
                StartCoroutine(InstructionsTimer());
            }
        }
    }

    void ResetSupportLayers()
    {
        textBackground.SetActive(false);
        introText.SetActive(false);

        audioSource.Stop();
        audioSource.clip = null;

        foreach (SingleTask task in instructionTasks)
        {
            task.supportText.SetActive(false);
            task.supportTextSmall.SetActive(false);
            task.supportLight.SetActive(false);
            task.supportGhost.SetActive(false);
        }

        foreach (SingleTask task in driveTasks)
        {
            task.supportText.SetActive(false);
            task.supportTextSmall.SetActive(false);
            task.supportLight.SetActive(false);
            task.supportGhost.SetActive(false);
        }

        foreach (SingleTask task in drillTasks)
        {
            task.supportText.SetActive(false);
            task.supportTextSmall.SetActive(false);
            task.supportLight.SetActive(false);
            task.supportGhost.SetActive(false);
        }

        foreach (SingleTask task in torchTasks)
        {
            task.supportText.SetActive(false);
            task.supportTextSmall.SetActive(false);
            task.supportLight.SetActive(false);
            task.supportGhost.SetActive(false);
        }
    }

    //Buttons

    public void ButtonDrive()
    {
        if (!introFinnished || !instructionsFinnished)
            return;

        ButtonMoodCheck(SupportMood.Drive);
    }

    public void ButtonDrill()
    {
        if (!introFinnished || !instructionsFinnished)
            return;

        ButtonMoodCheck(SupportMood.Drill);
    }

    public void ButtonTorch()
    {
        if (!introFinnished || !instructionsFinnished)
            return;

        ButtonMoodCheck(SupportMood.Torch);
    }

    void ButtonMoodCheck(SupportMood newMood)
    {
        ResetSupportLayers();

        if (supportMood != newMood)
        {
            SupportInstructions(newMood, default, true);
        }
        else
        {
            SupportInstructions(SupportMood.None);
        }
    }

    public void ButtonTextSupport()
    {
        CheckSupportToggle(supportTextToggle, ref supportlayerText);

        if (!introFinnished || !instructionsFinnished)
            return;

        ResetSupportLayers();
    }

    public void ButtonLightSupport()
    {
        CheckSupportToggle(supportLightToggle, ref supportlayerLight);

        if (!introFinnished || !instructionsFinnished)
            return;

        /*
        if (!instructionsFinnished)
        {
            TaskInstructions.instance.InstructionButton(this.gameObject);
        }
        */

        ResetSupportLayers();
    }

    public void ButtonVoiceSupport()
    {
        CheckSupportToggle(supportVoiceToggle, ref supportlayerVoice);

        if (!introFinnished || !instructionsFinnished)
            return;

        ResetSupportLayers();
    }

    public void ButtonGhostSupport()
    {
        CheckSupportToggle(supportGhostToggle, ref supportlayerGhost);

        if (!introFinnished || !instructionsFinnished)
            return;

        ResetSupportLayers();
    }

    public void ButtonAnimatedControlers()
    {
        animatedContolersIsOn = !animatedContolersIsOn;
        supportAnimatedControlers.SetActive(animatedContolersIsOn);
    }

    public void ButtonTrainingObjectives()
    {
        objectiveCanvasIsOn = !objectiveCanvasIsOn;
        objectivesCanvas.SetActive(objectiveCanvasIsOn);

        
        if (objectiveCanvasIsOn && !instructionsFinnished)
        {
            objectiveRoutine = StartCoroutine(ObjectiveAudio());
        }
        else
        {
            if (objectiveRoutine != null)
                StopCoroutine(objectiveRoutine);
        }
    }

    IEnumerator ObjectiveAudio()
    {
        if (objectiveRoutine != null)
            StopCoroutine(objectiveRoutine);

        objectiveAudioSource.clip = objectiveClip;
        objectiveAudioSource.PlayOneShot(objectiveClip);

        yield return new WaitForSeconds(objectiveClip.length);
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

        
        if (supportMenu.activeSelf == true)
        {
            Time.timeScale = 0.0f;
            lastCoroutine = StartCoroutine(PlayAudio(objectiveClip));
        }
        else
        {
            Time.timeScale = 1.0f;
            if (lastCoroutine != null)
                StopCoroutine(lastCoroutine);
        }
        


        if (!instructionsFinnished && index == 4)
        {
            xButtonPressed = true;
            SupportInstructions(SupportMood.Introduction);
        }
    }

    IEnumerator InstructionsTimer()
    {
        switch (index)
        {
            case 2:
                {
                    ResetSupportLayers();
                    taskCompleteSource.PlayOneShot(taskCompleteClip, 0.2f);
                    yield return new WaitForSeconds(objectiveClip.length);

                    ButtonTrainingObjectives();
                    textBackground.SetActive(true);
                    instructionTasks[index - 1].supportText.SetActive(true);
                    instructionTasks[index - 1].supportTextSmall.SetActive(true);
                    lastCoroutine = StartCoroutine(PlayAudio(instructionTasks[index - 1].supportVoice));
                    break;
                }
            case 5:
                {
                    yield return new WaitForSeconds(instructionTasks[4].supportVoice.length);
                    SupportInstructions(SupportMood.Introduction);
                    break;
                }
            case 6:
                {
                    yield return new WaitForSeconds(instructionTasks[5].supportVoice.length);
                    SupportInstructions(SupportMood.Introduction);
                    break;
                }
        }
    }

    IEnumerator ShowObjectTimer(GameObject someGameObject,float showTime, float beginDelay = 0)
    {
        yield return new WaitForSeconds(beginDelay);
        someGameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        someGameObject.SetActive(false);
    }
}

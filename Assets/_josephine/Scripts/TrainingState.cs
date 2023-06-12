using BNG;
using Photon.Voice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingState : MonoBehaviour
{
    //Audio
    public AudioSource audioSource;
    public AudioClip progressClip;
    public AudioClip failureClip;
    public AudioClip damageClip;
    public AudioClip gemClip;
    Coroutine lastCoroutine;

    public GameObject megaGem;

    [Header("Damage Meter")]
    public Transform damageMeter;
    public Renderer damageIndicator;
    public Gradient damageColor;
    public Gradient emissiveColor;
    [SerializeField]
    int maxDamage = 100;
    [SerializeField]
    int curDamage = 0;
    float meterSize;
    bool damageMeterPulsatingOn = false;
    public float duration = 2.0f;

    //Training figures
    Coroutine lastPopUp = null;
    public GameObject popUpMessage;
    public TextMeshProUGUI popUpText;
    int neededTrainingIterations;
    int currentTrainingIteration = 0;

    int trainingProgress = 0;
    int trainingFailures = 0;
    int trainingScore = 0;
    int scoreMultiplier = 1;
    int gemsCount = 0;
    int trollsKill = 0;

    //Task figures
    int taskDriveProgress = 0;
    int taskDriveFailures = 0;
    int taskDrillProgress = 0;
    int taskDrillFailures = 0;
    int taskTorchProgress = 0;
    int taskTorchFailures = 0;

    //UI
    [Header("Training info")]
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTrainingFailures;
    public TextMeshProUGUI textTrainingScore;
    public TextMeshProUGUI textTrainingScoreMultiplier;
    public TextMeshProUGUI textTrainingGemsCount;
    public TextMeshProUGUI textTrainingTrollsKill;

    [Header("Task info")]
    public TextMeshProUGUI textTaskDriveProgress;
    public TextMeshProUGUI textTaskDriveFailures;
    public TextMeshProUGUI textTaskDrillProgress;
    public TextMeshProUGUI textTaskDrillFailures;
    public TextMeshProUGUI textTaskTorchProgress;
    public TextMeshProUGUI textTaskTorchFailures;

    //Singelton
    public static TrainingState instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //StartCoroutine(PlayerStartPosition());
        neededTrainingIterations = TaskDrive.instance.neededIterations + TaskDrill.instance.neededIterations + TaskTorch.instance.neededIterations;

        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        textTrainingFailures.text =  "Failures: " + trainingFailures;
        textTrainingScore.text = "Training score: " + trainingScore;
        textTrainingScoreMultiplier.text = "Multiplier: " + scoreMultiplier.ToString() + "X";

        textTaskDriveProgress.text = taskDriveProgress + "%";
        textTaskDriveFailures.text = taskDriveFailures.ToString();

        megaGem.SetActive(false);
        popUpMessage.SetActive(false);
        UpdateDamageMeter();
    }

    /*
    IEnumerator PlayerStartPosition()
    {
        
        yield return new WaitForSeconds(1);

        if (smoothLocomotionChanged)
        {
            smoothLocomotionChanged = false;
            playerPosition[0].GetComponentInParent<SmoothLocomotion>().enabled = false;
        }

        foreach (Transform t in playerPosition)
        {
            playerStartPosition.Add(t.position);
        }
        positionSet = true;
    }
    */

    
    void LateUpdate()
    {
        if (!damageMeterPulsatingOn)
        {
            var amplitude = Mathf.PingPong(Time.time, duration);
            amplitude = amplitude / duration * 0.5f + 1.0f;
            damageIndicator.material.SetColor("_EmissionColor", emissiveColor.Evaluate(meterSize) * 5.5f * amplitude);
        }
        else
        {
            var amplitude = Mathf.PingPong(Time.time, duration / 3.0f);
            amplitude = amplitude / duration * 0.2f + 1.0f;
            damageIndicator.material.SetColor("_EmissionColor", emissiveColor.Evaluate(meterSize) * 5.5f * amplitude);
        }
    }
    

    public void UpdateTaskDriveProgress(int neededIt, int currentIt)
    {
        textTaskDriveProgress.text = Percentage(neededIt, currentIt).ToString() + "%";
        UpdateTrainingProgress();
        lastPopUp = StartCoroutine(ShowPopUpMessage("Drive progress: " + Percentage(neededIt, currentIt).ToString()));
    }

    public void UpdateTaskDrillProgress(int neededIt, int currentIt)
    {
        currentIt++;
        textTaskDrillProgress.text = Percentage(neededIt, currentIt).ToString() + "%";
        UpdateTrainingProgress();
        lastPopUp = StartCoroutine(ShowPopUpMessage("Drill progress: " + Percentage(neededIt, currentIt).ToString()));
    }

    public void UpdateTaskTorchProgress(int neededIt, int currentIt)
    {
        string progress = Percentage(neededIt, currentIt).ToString();
        textTaskTorchProgress.text = progress + "%";
        trollsKill++;
        textTrainingTrollsKill.text = "Trolls Kill: " + trollsKill.ToString();
        UpdateTrainingProgress();
        lastPopUp = StartCoroutine(ShowPopUpMessage("Torch progress: " + progress + "%\nTrolls Kill: +1"));
    }

    int Percentage(int neededIt, int currentIt)
    {
        return (int)((float)currentIt / (float)neededIt * 100);
    }

    void UpdateTrainingProgress()
    {
        currentTrainingIteration++;
        trainingProgress = Percentage(neededTrainingIterations, currentTrainingIteration);
        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        lastCoroutine = StartCoroutine(PlayAudio(gemClip, 0.2f));

        if (currentTrainingIteration == neededTrainingIterations)
        {
            Debug.Log("Training finnished!");
            Invoke("TrainingFinnished", 2);
            //Spawn mega gem
            megaGem.SetActive(true);
        }
    }

    void TrainingFinnished()
    {
        //SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void UpdateScoreMultiplier()
    {
        scoreMultiplier += 1;
        textTrainingScoreMultiplier.text = "Multiplier: " + scoreMultiplier.ToString() + "X";
    }

    public void UpdateTrainingScore(int gemScore)
    {
        lastCoroutine = StartCoroutine(PlayAudio(gemClip, 0.2f));
        trainingScore += gemScore * scoreMultiplier;
        gemsCount++;
        textTrainingScore.text = "Score: " + trainingScore;
        textTrainingGemsCount.text = "Gems: " + gemsCount.ToString();
        lastPopUp = StartCoroutine(ShowPopUpMessage("Score: +" + (gemScore * scoreMultiplier) + "\nGems: +1"));
    }

    public void UpdateTrainingFailures()
    {
        trainingFailures++;
        textTrainingFailures.text = "Failures: " + trainingFailures;
        scoreMultiplier = 1;
        lastCoroutine = StartCoroutine(PlayAudio(failureClip, 0.6f));
        lastPopUp = StartCoroutine(ShowPopUpMessage("Failures: +1 \nScore multiplier: 1X"));
    }

    public void UpdateDriveFailure()
    {
        taskDriveFailures++;
        textTaskDriveFailures.text = taskDriveFailures.ToString();
    }

    public void UpdateDrillFailure()
    {
        taskDrillFailures++;
        textTaskDrillFailures.text = taskDrillFailures.ToString();
    }

    public void UpdateTorchFailure()
    {
        taskTorchFailures++;
        textTaskTorchFailures.text = taskTorchFailures.ToString();
    }

    public void MachineDamage(int damage)
    {
        if (damage <= 0) return;

        curDamage += damage;
        UpdateDamageMeter();

        lastPopUp = StartCoroutine(ShowPopUpMessage("Damage: +" + damage));
        lastCoroutine = StartCoroutine(PlayAudio(damageClip, 0.1f));

        if (curDamage >= maxDamage)
        {
            trainingScore = 0;

            //You fired screen
        }
    }

    void UpdateDamageMeter()
    {
        float damageSize = 1.0f / (float)maxDamage;
        meterSize = ((float)maxDamage - (float)curDamage) * damageSize;

        if (meterSize >= 0)
        {
            damageMeter.localScale = new Vector3(meterSize, 1, 1);
            damageIndicator.material.color = damageColor.Evaluate(meterSize);
            lastPopUp = StartCoroutine(MeterBlink(meterSize));
        }

        if (meterSize < 0.5f)
        {
            damageMeterPulsatingOn = true;
        }
    }

    IEnumerator MeterBlink(float meterSize)
    {
        damageIndicator.material.SetColor("_EmissionColor", emissiveColor.Evaluate(meterSize) * 15.9f);
        yield return new WaitForSeconds(0.6f);
        damageIndicator.material.SetColor("_EmissionColor", emissiveColor.Evaluate(meterSize) * 5.5f);
    }

    IEnumerator PlayAudio(AudioClip clip, float volume)
    {
        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        //audioSource.Stop();
        //audioSource.clip = null;
    }

    IEnumerator ShowPopUpMessage(string message)
    {
        if (lastPopUp != null) 
            StopCoroutine(lastPopUp);

        popUpText.text = message;
        popUpMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        popUpMessage.SetActive(false);
    }
}
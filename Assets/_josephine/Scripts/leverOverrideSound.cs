using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverOverrideSound : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(QuietAtStart());
    }

    IEnumerator QuietAtStart()
    {
        audioSource.volume = 0.0f;
        yield return new WaitForSeconds(1);
        audioSource.volume = 0.2f;
    }
}

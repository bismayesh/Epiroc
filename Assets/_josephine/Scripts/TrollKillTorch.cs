using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollKillTorch : MonoBehaviour
{
    public GameObject lightPulse;
    public List<Troll> trolls = new List<Troll>();

    public static TrollKillTorch instance;

    private void Awake()
    {
        instance = this;
    }

    public void EmptyTrollsList()
    {
        trolls.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Troll"))
        {
            trolls.Add(other.gameObject.GetComponent<Troll>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Troll"))
        {
            trolls.Remove(other.gameObject.GetComponent<Troll>());
        }
    }

    public void FireLightPulse()
    {
        //Instantiate(lightPulse);


        StartCoroutine(DelayTrollKill());  
    }

    IEnumerator DelayTrollKill()
    {
        yield return new WaitForSeconds(1);

        foreach (Troll troll in trolls)
        {
            trolls.Remove(troll);
            TaskTorch.instance.TorchProgress();
            troll.DestroyTroll();
        }
    }
}

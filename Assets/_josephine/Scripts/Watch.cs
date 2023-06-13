using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Watch : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    [HideInInspector]
    public string playTime;
    public static Watch instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        string Shours = TimeToString(3600);
        string Sminutes = TimeToString(60);
        string Sseconds = TimeToString(1);

        /*
        //Seconds
        int sec = (int)(Time.realtimeSinceStartup);
        sec = sec % 60;
        if (sec == 0) Sseconds = "00";
        else if (sec > 0 && sec <= 9) Sseconds = "0" + sec;
        else Sseconds = sec.ToString();
        

        //minutes
        int min = (int)(Time.realtimeSinceStartup);
        min = min % 3600;
        Sminutes = min.ToString();
        */

        playTime = Shours + ":" + Sminutes + ":" + Sseconds;
        timeText.text = playTime;
    }

    string TimeToString(int timeToSec)
    {
        string Stime;
        int time = (int)(Time.realtimeSinceStartup / (float)timeToSec);
        time = time % 60;

        if (time == 0) Stime = "00";
        else if (time > 0 && time <= 9) Stime = "0" + time.ToString();
        else Stime = time.ToString();

        return Stime;
    }
}

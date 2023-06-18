using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstructions : Task
{
    public List<SingleTask> taskControl = new List<SingleTask>();


    //Singelton
    public static TaskInstructions instance;

    public bool IntroductionMood
    {
        get { return taskControl[0].IsOn; }
    }

    private void Awake()
    {
        instance = this;
    }

    /*
    public void InstructionButton(GameObject thisObject)
    {
        foreach (SingleTask singleTask in taskControl)
        {
            if (thisObject == singleTask.task && !singleTask.IsOn)
            {
                TaskCheck(thisObject, true);
            }
            else if (thisObject == singleTask.task && singleTask.IsOn)
            {
                TaskCheck(thisObject, false);
            }
        }
    }
    */

    public void TaskCheck(GameObject thisObject, bool setOn)
    {
        for (int x = 0; x < taskControl.Count; x++)
        {
            taskControl[x].TaskCheck(thisObject, setOn);
        }
        /*
        if (taskControl[0].TaskCheck(thisObject, setOn))
            JacksLever();
        if (taskControl[1].TaskCheck(thisObject, setOn))
            FrontJacks();
        if (taskControl[2].TaskCheck(thisObject, setOn))
            RearJacks();
        if (taskControl[3].TaskCheck(thisObject, setOn))
            DrillLever();
        */
    }

    private void JacksLever()
    {
        if (taskControl[0].IsOn)
        {

        }
        else
        {

        }
    }

    private void FrontJacks()
    {
        if (taskControl[1].IsOn)
        {

        }
        else
        {

        }
    }

    private void RearJacks()
    {
        if (taskControl[2].IsOn)
        {

        }
        else
        {

        }
    }

    private void DrillLever()
    {
        if (taskControl[3].IsOn)
        {

        }
        else
        {

        }
    }
}

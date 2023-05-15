using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatistics
{
    public static int TrainingAttempts = 0;
    public static int TrainingFailures = 0;

    public static void UpdateTrainingAttempts()
    {
        TrainingAttempts++;
    }

    public static void UpdateTrainingFailures()
    {
        TrainingFailures++;
    }
}

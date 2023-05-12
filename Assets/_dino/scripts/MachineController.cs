using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class MachineController : MonoBehaviour {

    #region Events

    public static event Action OnJacksRetrieved;
    public static event Action OnDrillActivated;
    public static event Action OnBrakesReleased;
    public static event Action OnDrillSpinning;

    #endregion
    #region References

    [FoldoutGroup("References")] public DrillController drill;
    [FoldoutGroup("References")] public List<GameObject> jacks;

    #endregion
    #region Bools

    [HideInInspector] public bool jacksRetrieved;
    [HideInInspector] public bool drillActive;
    [HideInInspector] public bool brakesReleased;

    #endregion
    #region DrillVariables

    [PropertyOrder(1)][TabGroup("Drill Controlls")] [Range(0,1)] public float drillSliderPosition;
    [PropertyOrder(1)][TabGroup("Drill Controlls")] [Range(0,1)] public float drillSliderAngle;
    [PropertyOrder(1)][TabGroup("Drill Controlls")] [Range(0,1)] public float drillArmAngle;
    [PropertyOrder(1)][TabGroup("Drill Controlls")] [Range(0,1)] public float drillCaseAngle;
    [PropertyOrder(1)][TabGroup("Drill Controlls")] [Range(0,1)] public float drillCaseRotation;

    #endregion
    #region MachineVariables

    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(0, 1)] public float machineSpeed;
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(0, 1)] public float machineRotation;

    #endregion

    #region Functions

    [PropertyOrder(0)][BoxGroup("Functionality")][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void RetrieveJacks() {
        StartCoroutine(CO_RetrieveJacks());
    }

    [PropertyOrder(0)][BoxGroup("Functionality")][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ReleaseBrakes() {
        StartCoroutine(CO_ReleaseBrakes());
    }
    
    [PropertyOrder(0)][BoxGroup("Functionality")][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateDrill() {
        StartCoroutine(CO_ActivateDrill());
    }
    
    [PropertyOrder(0)][BoxGroup("Functionality")][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void SpinDrill() {
        StartCoroutine(CO_SpinDrill());
    }
    

    #endregion

    #region Coroutines

    IEnumerator CO_RetrieveJacks() {
        
        OnJacksRetrieved?.Invoke();
        yield return null;
    }

    IEnumerator CO_ReleaseBrakes() {
        
        OnBrakesReleased?.Invoke();
        yield return null;
    }

    IEnumerator CO_ActivateDrill() {
        
        OnDrillActivated?.Invoke();
        yield return null;
    }

    IEnumerator CO_SpinDrill() {
        
        OnDrillSpinning?.Invoke();
        yield return null;
    }

    #endregion
}

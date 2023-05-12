using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MachineController : MonoBehaviour {

    #region Events
    
    
    [Space(30)]
    [HorizontalGroup("Jack EVENTS")][PropertyOrder(666)] public UnityEvent OnJacksRetrieved;
    [Space(30)]
    [HorizontalGroup("Jack EVENTS")][PropertyOrder(666)] public UnityEvent OnJacksExtended;
    [HorizontalGroup("Drill EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillActivated;
    [HorizontalGroup("Drill EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillDeactivated;
    [HorizontalGroup("Brakes EVENTS")][PropertyOrder(666)] public UnityEvent OnBrakesReleased;
    [HorizontalGroup("Brakes EVENTS")][PropertyOrder(666)] public UnityEvent OnBrakesActivated;
    [HorizontalGroup("Drill spin EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillSpinning;
    [HorizontalGroup("Drill spin EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillStop;

    #endregion
    #region References

    [FoldoutGroup("References")] public DrillController Drill;
    [FoldoutGroup("References")] public List<GameObject> Jacks;

    #endregion
    #region Bools

    [HideInInspector] public bool MachineActive;
    [HideInInspector] public bool JacksRetrieved;
    [HideInInspector] public bool DrillActive;
    [HideInInspector] public bool BrakesReleased;
    [HideInInspector] public bool DrillSpinning;

    #endregion
    #region MachineVariables
    
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(0, 1)] public float MachineMovementSpeed;
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(0, 1)] public float MachineRotationSpeed;

    #endregion
    #region DrillVariables

    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, 15)] public float CaseJointRotation;
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, -90)] public float SliderJointRotation;
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, -90)] public float SliderPosition;

    #endregion

    #region MachineAndDrillFunctions

    
    
    
    [DisableIf("@JacksRetrieved")]
    [PropertyOrder(0)]
    [HorizontalGroup("0")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void RetrieveJacks() {
        StartCoroutine(CO_RetrieveJacks());
        
    }
    
    [DisableIf("@!JacksRetrieved")]
    [PropertyOrder(0)]
    [HorizontalGroup("0")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ExtendJacks() {
        StartCoroutine(CO_ExtendJacks());
        
    }
    
    [DisableIf("@BrakesReleased")]
    [PropertyOrder(0)]
    [HorizontalGroup("1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ReleaseBrakes() {
        StartCoroutine(CO_ReleaseBrakes());
        
    }
    [DisableIf("@!BrakesReleased")]
    [PropertyOrder(0)]
    [HorizontalGroup("1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateBrakes() {
        StartCoroutine(CO_ActivateBrakes());
        
    }
    
    [DisableIf("@DrillActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("2")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateDrill() {
        StartCoroutine(CO_ActivateDrill());
        
    }
    [DisableIf("@!DrillActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("2")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void DeactivateDrill() {
        StartCoroutine(CO_DeactivateDrill());
        
    }
    
    [DisableIf("@DrillSpinning")]
    [PropertyOrder(0)]
    [HorizontalGroup("3")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void SpinDrill() {
        StartCoroutine(CO_SpinDrill());
        
    }
    [DisableIf("@!DrillSpinning")]
    [PropertyOrder(0)]
    [HorizontalGroup("3")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void StopDrill() {
        StartCoroutine(CO_StopDrill());
        
    }
    
    

    #endregion
    #region MachineAndDrillCoroutines

    IEnumerator CO_RetrieveJacks() {
        foreach (var jack in Jacks) {
            jack.transform.DOLocalMoveX(-0.5f, 5);
        }
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        JacksRetrieved = true;
        OnJacksRetrieved?.Invoke();
    }
    
    IEnumerator CO_ExtendJacks() {
        foreach (var jack in Jacks) {
            jack.transform.DOLocalMoveX(0.5f, 5);
        }
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        JacksRetrieved = false;
        OnJacksExtended?.Invoke();
        yield return null;
    }

    IEnumerator CO_ReleaseBrakes() {
        
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        BrakesReleased = true;
        OnBrakesReleased?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_ActivateBrakes() {
        
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        BrakesReleased = false;
        OnBrakesActivated?.Invoke();
        yield return null;
    }

    IEnumerator CO_ActivateDrill() {
        var endRotation = Vector3.zero;
        Drill.SliderJoint.transform.DORotate(endRotation, 4);
        Drill.CaseJoint.transform.DORotate(endRotation, 6);

        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        DrillActive = true;
        OnDrillActivated?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_DeactivateDrill() {
        var endRotationSliderJoint = new Vector3(-30, 0, 0);
        var endRotationCaseJoint = new Vector3(15, 0, 0);
        Drill.SliderJoint.transform.DORotate(endRotationSliderJoint, 4);
        Drill.CaseJoint.transform.DORotate(endRotationCaseJoint, 6);

        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        DrillActive = false;
        OnDrillDeactivated?.Invoke();
        yield return null;
    }

    IEnumerator CO_SpinDrill() {
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        DrillSpinning = true;
        OnDrillSpinning?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_StopDrill() {
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        DrillSpinning = false;
        OnDrillStop?.Invoke();
        yield return null;
    }

    #endregion
    
}

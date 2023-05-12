using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class MachineController : MonoBehaviour {

    #region Events
    
    [Space(30)]
    [HorizontalGroup("Engine EVENTS")][PropertyOrder(666)] public UnityEvent OnEngineActivated;
    [Space(30)]
    [HorizontalGroup("Engine EVENTS")][PropertyOrder(666)] public UnityEvent OnEngineDeactivated;
    [HorizontalGroup("Jack EVENTS")][PropertyOrder(666)] public UnityEvent OnJacksRetrieved;
    [HorizontalGroup("Jack EVENTS")][PropertyOrder(666)] public UnityEvent OnJacksExtended;
    [HorizontalGroup("Drill EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillActivated;
    [HorizontalGroup("Drill EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillDeactivated;
    [HorizontalGroup("Brakes EVENTS")][PropertyOrder(666)] public UnityEvent OnBrakesReleased;
    [HorizontalGroup("Brakes EVENTS")][PropertyOrder(666)] public UnityEvent OnBrakesActivated;
    [HorizontalGroup("Drill spin EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillSpinning;
    [HorizontalGroup("Drill spin EVENTS")][PropertyOrder(666)] public UnityEvent OnDrillStop;

    #endregion
    #region References

    [PropertyOrder(-666)][FoldoutGroup("References")] public DrillController Drill;
    [PropertyOrder(-666)][FoldoutGroup("References")] public Rigidbody Engine;
    [PropertyOrder(-666)][FoldoutGroup("References")] public HingeJoint WheelL;
    [PropertyOrder(-666)][FoldoutGroup("References")] public HingeJoint WheelR;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> Jacks;

    #endregion
    #region Bools

    [HideInInspector]public bool EngineActive;
    [HideInInspector]public bool JacksRetrieved;
    [HideInInspector]public bool DrillActive;
    [HideInInspector]public bool BrakesReleased;
    [HideInInspector]public bool DrillSpinning;

    #endregion
    #region MachineVariables
    
    [ShowIf("@EngineActive")]
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(-100 , 100)] public float MachineMovementForce;
    [ShowIf("@EngineActive")]
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(-180, 180)] public float MachineRotationForce;

    #endregion
    #region DrillVariables

    
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, 30)] public float CaseJointRotation;
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(-90, 0)] public float SliderJointRotation;
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(-1.75f, 1.75f)] public float SliderPosition;

    #endregion

    #region MachineAndDrillFunctions
    
    
    
    [DisableIf("@EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("-1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateEngine() {
        StartCoroutine(CO_ActivateEngine());
        
    }
    [DisableIf("@!EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("-1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void DeactivateEngine() {
        StartCoroutine(CO_DeactivateEngine());
        
    }
    
    
    [DisableIf("@JacksRetrieved || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("0")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void RetrieveJacks() {
        StartCoroutine(CO_RetrieveJacks());
        
    }
    
    [DisableIf("@!JacksRetrieved || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("0")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ExtendJacks() {
        StartCoroutine(CO_ExtendJacks());
        
    }
    
    [DisableIf("@BrakesReleased || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ReleaseBrakes() {
        StartCoroutine(CO_ReleaseBrakes());
        
    }
    [DisableIf("@!BrakesReleased || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("1")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateBrakes() {
        StartCoroutine(CO_ActivateBrakes());
        
    }
    
    [DisableIf("@DrillActive || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("2")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void ActivateDrill() {
        StartCoroutine(CO_ActivateDrill());
        
    }
    [DisableIf("@!DrillActive || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("2")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void DeactivateDrill() {
        StartCoroutine(CO_DeactivateDrill());
        
    }
    
    [DisableIf("@DrillSpinning || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("3")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void SpinDrill() {
        StartCoroutine(CO_SpinDrill());
        
    }
    [DisableIf("@!DrillSpinning || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("3")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void StopDrill() {
        StartCoroutine(CO_StopDrill());
    }
    
    

    #endregion
    #region MachineAndDrillCoroutines

    IEnumerator CO_ActivateEngine() {
        
        EngineActive = true;
        OnEngineActivated?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_DeactivateEngine() {
        
        EngineActive = false;
        OnEngineDeactivated?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_RetrieveJacks() {
        foreach (var jack in Jacks) {
            jack.transform.DOLocalMoveX(-0.75f, 5);
        }
        
        JacksRetrieved = true;
        OnJacksRetrieved?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_ExtendJacks() {
        foreach (var jack in Jacks) {
            jack.transform.DOLocalMoveX(0.5f, 5);
        }
        
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
        var endRotation = new Vector3(0, 0, 0);
        Drill.SliderJoint.transform.DOLocalRotate(endRotation, 4);
        Drill.CaseJoint.transform.DOLocalRotate(endRotation, 6);

        yield return new WaitForSeconds(6);
        DrillActive = true;
        CaseJointRotation = 0;
        SliderJointRotation = 0;
        SliderPosition = 1.75f;
        OnDrillActivated?.Invoke();
        
    }
    
    IEnumerator CO_DeactivateDrill() {
        var endRotationSliderJoint = new Vector3(-30, 0, 0);
        var endRotationCaseJoint = new Vector3(15, 0, 0);
        Drill.SliderJoint.transform.DOLocalRotate(endRotationSliderJoint, 4);
        Drill.CaseJoint.transform.DOLocalRotate(endRotationCaseJoint, 6);

        yield return new WaitForSeconds(6);
        DrillActive = false;
        OnDrillDeactivated?.Invoke();
        
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

    private void Start() {
        
    }

    private void Update() {
        if (BrakesReleased){
            var motorL = WheelL.motor;
            var motorR = WheelR.motor;

            motorL.force = 100;
            motorL.targetVelocity = MachineMovementForce;
            motorL.freeSpin = false;
            WheelL.motor = motorL;
            WheelL.useMotor = true;
            
            motorR.force = 100;
            motorR.targetVelocity = MachineMovementForce;
            motorR.freeSpin = false;
            WheelR.motor = motorR;
            WheelR.useMotor = true;

            Engine.AddTorque(Vector3.up * MachineRotationForce);
        }
        else{
            var motorL = WheelL.motor;
            var motorR = WheelR.motor;

            motorL.force = 0;
            motorL.targetVelocity = MachineMovementForce;
            motorL.freeSpin = false;
            WheelL.motor = motorL;
            WheelL.useMotor = true;
            
            motorR.force = 0;
            motorR.targetVelocity = MachineMovementForce;
            motorR.freeSpin = false;
            WheelR.motor = motorR;
            WheelR.useMotor = true;
        }
        
        if (DrillActive) {
            Drill.CaseJoint.transform.localEulerAngles = new Vector3((float)Math.Round(CaseJointRotation, 2), 0, 0);
            Drill.SliderJoint.transform.localEulerAngles = new Vector3((float)Math.Round(SliderJointRotation, 2), 0, 0);
            Drill.Slider.transform.localPosition = new Vector3(0, 0.375f, (float)Math.Round(SliderPosition, 2));

        }
        else {
            CaseJointRotation = (float)Math.Round(Drill.CaseJoint.transform.eulerAngles.x, 2);
            SliderJointRotation = (float)Math.Round(Drill.SliderJoint.transform.eulerAngles.x / 360, 2);
            SliderPosition = (float)Math.Round(Drill.Slider.transform.localPosition.z, 2);
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using static UnityEngine.Rendering.DebugUI;

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

    [HorizontalGroup("Drill spin EVENTS")] [PropertyOrder(666)]
    public UnityEvent OnTrollHit;

    #endregion
    #region References

    [PropertyOrder(-666)][FoldoutGroup("References")] public DrillController Drill;
    [PropertyOrder(-666)][FoldoutGroup("References")] public Light Torch;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksLeft;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksLeftMid;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksLeftLow;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksRight;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksRightMid;
    [PropertyOrder(-666)][FoldoutGroup("References")] public List<GameObject> JacksRightLow;


    #endregion

    #region Bools

    [HideInInspector]public bool EngineActive;
    [HideInInspector]public bool JacksRetrieved;
    [HideInInspector]public bool DrillActive;
    [HideInInspector]public bool BrakesReleased;
    [HideInInspector]public bool DrillSpinning;

    #endregion
    #region MachineVariables

    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(-100 , 100)] public float MachineMovementForce;
    [PropertyOrder(1)] [TabGroup("Machine Controlls")] [Range(-180, 180)] public float MachineRotationForce;

    #endregion
    #region DrillVariables
    
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, 30)] public float CaseJointRotation;
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(-90, 0)] public float SliderJointRotation;
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(-1.75f, 1.75f)] public float SliderPosition;
    [ShowIf("@DrillActive")]
    [PropertyOrder(1)] [TabGroup("Drill Controlls")] [Range(0, 100)] public float DrillSpinSpeed;



    #endregion

    #region TorchVariables

    public float torchSpeed = 5.0f;
    public float torchIntensity;
    public float torchSpread;
    public bool isTorchActive;
    
    #endregion
    #region MachineAndDrillFunctions
    
    public void ChangeMovementForce(Vector2 force){
        transform.Translate(Vector3.forward * force.y * MachineMovementForce * Time.deltaTime * 3);
    }

    public void ChangeRotationForce(Vector2 force){
        transform.Rotate(Vector3.up * force.x * MachineRotationForce * Time.deltaTime,Space.Self);
    }
    
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
        DrillSpinning = true;
        OnDrillSpinning?.Invoke();
    }
    [DisableIf("@!DrillSpinning || !EngineActive")]
    [PropertyOrder(0)]
    [HorizontalGroup("3")]
    [DisableInEditorMode][Button(ButtonSizes.Large), GUIColor(0,1,0)]
    public void StopDrill() {
        DrillSpinning = false;
        OnDrillStop?.Invoke();
    }

    public void ActivateTorch() { // button to activate torch
        isTorchActive = true;
    }
    
    public void DeactivateTorch() { // button to deactivate torhc
        isTorchActive = false;
    }

    public void ChangeTorchRotationX(Vector2 rot) { // plug this function in on the right joystick
        //var torchEuler = Torch.transform.localEulerAngles;
        //Torch.transform.eulerAngles += new Vector3(rot.x, torchEuler.y);
        Torch.transform.localEulerAngles += new Vector3(rot.y * torchSpeed * Time.deltaTime, 0, 0);
        
    }
    
    public void ChangeTorchRotationY(Vector2 rot) { // plug this function in on the left joystick
        //var torchEuler = Torch.transform.localEulerAngles;
        Torch.transform.localEulerAngles += new Vector3( 0, rot.x * torchSpeed * Time.deltaTime, 0);
    }

    public void ResetRotation()
    {
        Torch.transform.Rotate(0, 0, 0);
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
        foreach (var jack in JacksLeft) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 32), 5);
        }
        foreach (var jack in JacksLeftMid) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 43), 5);
        }
        foreach (var jack in JacksLeftLow) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, -12), 5);
        }
        foreach (var jack in JacksRight) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, -32), 5);
        }
        foreach (var jack in JacksRightMid) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 32), 5);
        }
        foreach (var jack in JacksRightLow) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 12), 5);
        }
        
        
        JacksRetrieved = true;
        OnJacksRetrieved?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_ExtendJacks() {
        foreach (var jack in JacksLeft) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 107), 5);
        }
        foreach (var jack in JacksLeftMid) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, -2), 5);
        }
        foreach (var jack in JacksLeftLow) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, -102), 5);
        }
        foreach (var jack in JacksRight) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, -107), 5);
        }
        foreach (var jack in JacksRightMid) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 2), 5);
        }
        foreach (var jack in JacksRightLow) {
            jack.transform.DOLocalRotate(new Vector3(0, 0, 102), 5);
        }
        
        JacksRetrieved = false;
        OnJacksExtended?.Invoke();
        yield return null;
    }

    IEnumerator CO_ReleaseBrakes() {
        
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        BrakesReleased = true;
        MachineMovementForce = 4;
        OnBrakesReleased?.Invoke();
        yield return null;
    }
    
    IEnumerator CO_ActivateBrakes() {
        
        yield return new WaitUntil((() => !DOTween.IsTweening(this)));
        BrakesReleased = false;
        MachineRotationForce = 32;
        OnBrakesActivated?.Invoke();
        yield return null;
    }

    IEnumerator CO_ActivateDrill() {
        var endRotation = new Vector3(0, 0, 0);
        Drill.Slider.transform.DOLocalRotate(endRotation, 4);
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
        Drill.Slider.transform.DOLocalRotate(endRotationSliderJoint, 4);
        Drill.CaseJoint.transform.DOLocalRotate(endRotationCaseJoint, 6);

        yield return new WaitForSeconds(6);
        DrillActive = false;
        OnDrillDeactivated?.Invoke();
        
    }

    

    #endregion

    private void Update() {
        Torch.intensity = torchIntensity;
        Torch.spotAngle = torchSpread;

        if (DrillSpinning) {
            Drill.DrillTip.transform.Rotate(-Vector3.forward * DrillSpinSpeed);
        }

        RaycastHit hit;

        if (Physics.Raycast(Torch.transform.position, Torch.transform.forward, out hit)) {
            if (hit.transform.CompareTag("Troll")) {
                OnTrollHit?.Invoke();

                //Destroy troll
                Destroy(hit.transform.gameObject);
            }
        }

        #region DebugStuff
        // testing ctrls for the machine
        if (Input.GetKey(KeyCode.W)) {
            ChangeMovementForce(new Vector2(0, -1));
        }
        if (Input.GetKey(KeyCode.A)) {
            ChangeRotationForce(new Vector2(-1, 0));
        }
        if (Input.GetKey(KeyCode.S)) {
            ChangeMovementForce(new Vector2(0, 1));
        }
        if (Input.GetKey(KeyCode.D)) {
            ChangeRotationForce(new Vector2(1, 0));
        }

        #endregion
        
        

    }
}

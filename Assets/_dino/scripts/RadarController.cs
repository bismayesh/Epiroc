using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour {
     [FoldoutGroup("References")] public Image RadarHand;
     [FoldoutGroup("References")] public Image Arrow;
     [FoldoutGroup("References")] public GameObject FixedInterface;
     [FoldoutGroup("References")] public List<GameObject> Trolls;
     [FoldoutGroup("References")] public List<GameObject> Gems;
     [SerializeField] private List<Image> TrollsRadar;
     [SerializeField] private List<Image> GemsRadar;
     


     private void Start() {
          InvokeRepeating("Blip", 0, 2);
     }

     [Button("Blip")]
     void Blip() {
          RadarHand.DOFade(1, 0.1f);
          Vector3 halfRotation = new Vector3(0, 0, -360);
          RadarHand.DOFade(0, 1.5f);
          RadarHand.transform.DOLocalRotate(halfRotation, 1, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
     }

     private void Update() {
          FixedInterface.transform.localEulerAngles = Vector3.forward;
     }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour {
     [FoldoutGroup("References")] public Image Pivot;
     [FoldoutGroup("References")] public Image RadarHand;
     [FoldoutGroup("References")] public Image Arrow;
     [FoldoutGroup("References")] public GameObject Scanner;
     [FoldoutGroup("References")] public GameObject FixedInterface;
     [FoldoutGroup("References")] public CanvasGroup Display;
     [FoldoutGroup("References")] public Renderer Bleep;

     private void Start() {
          InvokeRepeating("Blip", 0, 2);
     }

     [Button("Blip")]
     void Blip() {
          RadarHand.DOFade(1, 0.1f);
          Vector3 radarHandRot = new Vector3(0, 0, -360);
          Vector3 scannerRot = new Vector3(0, 360, 0);
          RadarHand.DOFade(0, 6f);
          RadarHand.transform.DOLocalRotate(radarHandRot, 6, RotateMode.FastBeyond360).SetRelative(true)
               .SetEase(Ease.Linear);
          Scanner.transform.DORotate(scannerRot, 6, RotateMode.FastBeyond360).SetRelative(true)
               .SetEase(Ease.Linear);
     }
     
}

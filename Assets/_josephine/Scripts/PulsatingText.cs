using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingText : MonoBehaviour
{
    public RectTransform transform;
    void Start()
    {
        transform = GetComponent<RectTransform>();
        StartAnimation();
    }

    public void StartAnimation()
    {
        transform.DOScale(0.7f, 2.0f).SetLoops(-1, LoopType.Yoyo);
    }
}




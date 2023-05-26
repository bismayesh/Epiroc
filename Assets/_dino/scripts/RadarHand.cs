using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RadarHand : MonoBehaviour {
    public GameObject Gem;
    public GameObject Troll;
    
    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case "ChunkGem":
                var bleep = Instantiate(Gem, other.transform.position, Quaternion.identity);
                bleep.GetComponent<Renderer>().material.DOFade(0, 4);
                Destroy(bleep, 5);
                break;
            
            case "SuperGem":
                break;
            
            case "Troll":
                var troll = Instantiate(Troll, other.transform.position, Quaternion.identity);
                troll.GetComponent<Renderer>().material.DOFade(0, 4);
                Destroy(troll, 5);
                break;
        }
    }
}

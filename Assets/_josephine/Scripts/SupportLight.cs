using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportLight : MonoBehaviour
{
    [HideInInspector]
    public Light lightSource;
    float range;
    public float duration = 2.0f;
    //public float intensity;
    public Color normalColor;
    public Color progressColor;

    void Start()
    {
        lightSource = GetComponent<Light>();
        range = lightSource.range;
    }

    void Update()
    {
        var amplitude = Mathf.PingPong(Time.time, duration);

        // Transform from 0..duration to 0.5..1 range.
        amplitude = amplitude / duration * 0.5f + 0.2f;

        lightSource.range = range * amplitude;
    }
}

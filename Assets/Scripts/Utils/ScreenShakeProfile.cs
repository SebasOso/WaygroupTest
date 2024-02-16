using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScreenShakeProfile", menuName = "CameraShake/ScreenShakeProfile", order = 0)]
public class ScreenShakeProfile : ScriptableObject 
{
    [Header("Impulse Source Settings")]
    public float impactTime = 0.2f;
    public float impactForce = 0.2f;
    public Vector3 defaultVelocity = new Vector3(0f, 0f, 1f);
    public AnimationCurve impulseCurve;
    
    [Header("Impulse Listener Settings")]
    public float listenerAmplitude = 1f;
    public float listenerFrecuency = 1f;
    public float listenerDuration = 1f;
}

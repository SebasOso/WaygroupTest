using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;
    private CinemachineImpulseDefinition cinemachineImpulseDefinition;
    [SerializeField] private CinemachineImpulseListener cinemachineImpulseListener;
    [SerializeField] private CinemachineImpulseListener cinemachineImpulseListenerFreeLook;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CameraShake(CinemachineImpulseSource cinemachineImpulseSource, float impulseForce)
    {
        cinemachineImpulseSource.GenerateImpulseWithForce(impulseForce);
    }
    public void ScreenShakeFromProfile(CinemachineImpulseSource cinemachineImpulseSource, ScreenShakeProfile screenShakeProfile)
    {
        SetUpScreenShakeSettings(screenShakeProfile, cinemachineImpulseSource);
        cinemachineImpulseSource.GenerateImpulseWithForce(screenShakeProfile.impactForce);
    }
    public void ScreenShakeFromProfileFreeLook(CinemachineImpulseSource cinemachineImpulseSource, ScreenShakeProfile screenShakeProfile)
    {
        SetUpScreenShakeSettings(screenShakeProfile, cinemachineImpulseSource);
        SetUpScreenShakeSettingsFreeLook(screenShakeProfile, cinemachineImpulseSource);
        cinemachineImpulseSource.GenerateImpulseWithForce(screenShakeProfile.impactForce);
    }
    private void SetUpScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource cinemachineImpulseSource)
    {
        cinemachineImpulseDefinition = cinemachineImpulseSource.m_ImpulseDefinition;
        cinemachineImpulseDefinition.m_ImpulseDuration = profile.impactTime;
        cinemachineImpulseSource.m_DefaultVelocity = profile.defaultVelocity;
        cinemachineImpulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        cinemachineImpulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        cinemachineImpulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrecuency;
        cinemachineImpulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
    }
    private void SetUpScreenShakeSettingsFreeLook(ScreenShakeProfile profile, CinemachineImpulseSource cinemachineImpulseSource)
    {
        cinemachineImpulseDefinition = cinemachineImpulseSource.m_ImpulseDefinition;
        cinemachineImpulseDefinition.m_ImpulseDuration = profile.impactTime;
        cinemachineImpulseSource.m_DefaultVelocity = profile.defaultVelocity;
        cinemachineImpulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        cinemachineImpulseListenerFreeLook.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        cinemachineImpulseListenerFreeLook.m_ReactionSettings.m_FrequencyGain = profile.listenerFrecuency;
        cinemachineImpulseListenerFreeLook.m_ReactionSettings.m_Duration = profile.listenerDuration;
    }
}

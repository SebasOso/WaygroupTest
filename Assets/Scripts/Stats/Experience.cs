using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class Experience : MonoBehaviour, IJsonSaveable
{
    [SerializeField] float experiencePoints = 0;
    public event Action OnExperienceGained;
    public void GainExperience(float experienceGained)
    {
        experiencePoints += experienceGained;
        OnExperienceGained();
    }
    public float GetExperience()
    {
        return experiencePoints;
    }
    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(experiencePoints);
    }
    public void RestoreFromJToken(JToken state)
    {
        experiencePoints = state.ToObject<float>();
    }
}

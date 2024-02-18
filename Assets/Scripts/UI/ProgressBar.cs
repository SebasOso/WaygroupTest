using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Waygroup;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        slider.value = InputManager.Instance.throwTime;
    }
}

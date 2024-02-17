using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    [SerializeField] AudioClip floorLeft;
    [SerializeField] AudioClip floorRight;
    private RaycastHit raycastHit;
    [SerializeField] AudioSource audioSource;

    private float lastStepTime;
    public float stepCooldown = 0.5f;

    public void LeftFootStep()
    {
        if(GetComponent<Animator>().GetFloat("speed") <= 1)
        {
            return;
        }
        Vector3 rayDirection = new Vector3(0, -1, 0);
        Ray ray = new Ray(transform.position, rayDirection);
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            if(GetComponent<Animator>().GetFloat("speed") >= 0)
            {
                //MUD LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 11)
                {
                    TryPlayFootStep(floorLeft);
                }
            }
        }
    }
    public void RightFootStep()
    {
        if(GetComponent<Animator>().GetFloat("speed") <= 1)
        {
            return;
        }
        Vector3 rayDirection = new Vector3(0, -1, 0);
        Ray ray = new Ray(transform.position, rayDirection);
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            if(GetComponent<Animator>().GetFloat("speed") >= 0)
            {
                if(raycastHit.transform.gameObject.layer == 11)
                {
                    TryPlayFootStep(floorRight);
                }
            }
        }
    }
    private void TryPlayFootStep(AudioClip audioClip)
    {
        if (Time.time - lastStepTime > stepCooldown)
        {
            PlayFootStep(audioClip);
            lastStepTime = Time.time;
        }
    }
    public void PlayFootStep(AudioClip audioClip)
    {
       audioSource.clip = audioClip;
       audioSource.Play();
    }
}

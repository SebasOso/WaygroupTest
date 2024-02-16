using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    [SerializeField] AudioClip mudLeft;
    [SerializeField] AudioClip mudRight;
    [SerializeField] AudioClip woodLeft;
    [SerializeField] AudioClip woodRight;
    [SerializeField] AudioClip snowLeft;
    [SerializeField] AudioClip snowRight;
    [SerializeField] AudioClip waterLeft;
    [SerializeField] AudioClip waterRight;
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
                if(raycastHit.transform.gameObject.layer == 12)
                {
                    TryPlayFootStep(mudLeft);
                }
                //WOOD LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 13)
                {
                    TryPlayFootStep(woodLeft);
                }
                //SNOW LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 14)
                {
                    TryPlayFootStep(snowLeft);
                }
                //SEA LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 6)
                {
                    TryPlayFootStep(waterLeft);
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
                //MUD LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 12)
                {
                    TryPlayFootStep(mudRight);
                }
                //WOOD LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 13)
                {
                    TryPlayFootStep(woodRight);
                }
                //SNOW LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 14)
                {
                    TryPlayFootStep(snowRight);
                }
                //SEA LAYER MANAGER
                if(raycastHit.transform.gameObject.layer == 6)
                {
                    TryPlayFootStep(waterRight);
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

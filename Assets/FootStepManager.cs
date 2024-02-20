
using UnityEngine;

/// <summary>
/// Manages footstep sounds based on player movement.
/// </summary>
public class FootStepManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioClip floorLeft;
    [SerializeField] AudioClip floorRight;
    [SerializeField] AudioSource audioSource;

    //Variables for the logic
    private RaycastHit raycastHit;
    private float lastStepTime;
    public float stepCooldown = 0.5f;

    /// <summary>
    /// Plays the left footstep sound.
    /// </summary>
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
                if(raycastHit.transform.gameObject.layer == 11)
                {
                    TryPlayFootStep(floorLeft);
                }
            }
        }
    }

    /// <summary>
    /// Plays the right footstep sound.
    /// </summary>
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

    /// <summary>
    /// Attempts to play the footstep sound with a cooldown.
    /// </summary>
    private void TryPlayFootStep(AudioClip audioClip)
    {
        if (Time.time - lastStepTime > stepCooldown)
        {
            PlayFootStep(audioClip);
            lastStepTime = Time.time;
        }
    }

    /// <summary>
    /// Plays the footstep sound.
    /// </summary>
    public void PlayFootStep(AudioClip audioClip)
    {
       audioSource.clip = audioClip;
       audioSource.Play();
    }
}

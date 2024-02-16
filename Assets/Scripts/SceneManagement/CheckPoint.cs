using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class CheckPoint : MonoBehaviour, IJsonSaveable
{
    [SerializeField] float fadeWaitTime = 1f;
    [SerializeField] int checkCollected = 0;

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(checkCollected);
    }

    public void RestoreFromJToken(JToken state)
    {
        int check = state.ToObject<int>();
        checkCollected = check;
        if(checkCollected == 1)
        {
            UpdatePlayer();
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            checkCollected = 1;
            StartCoroutine(Transition());
        }
    }
    private IEnumerator Transition()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();
        yield return new WaitForSeconds(fadeWaitTime);
    }
    private void UpdatePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
    }
}

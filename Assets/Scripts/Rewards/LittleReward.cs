using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleReward : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] int moneyToAdd = 25;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<AudioSource>().clip = audioClip;
            GetComponent<AudioSource>().Play();
            other.GetComponent<MoneyManager>().AddMoney(moneyToAdd);
            StartCoroutine(HideForSeconds(1f));
        }    
    }
    private IEnumerator HideForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingWrapper : MonoBehaviour
{
    private GameObject player;
    const string defaultSaveFile = "data";
    private void Awake() 
    {
        StartCoroutine(LoadLastScene());
    }
    IEnumerator LoadLastScene() 
    {
        //Application.targetFrameRate = 60;
        yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutInmediate();
        yield return fader.FadeIn(0.2f);
        //Load();
    }
    public void Load()
    {
        GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
    }
    public void Save()
    {
        GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
    }
    public void Respawn()
    {
        StartCoroutine(Transition());
    }
    private IEnumerator Transition()
    {
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutInmediate();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Load();
        yield return fader.FadeIn(1f);
    }
}

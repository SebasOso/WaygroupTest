using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashNormal01;
    [SerializeField] private ParticleSystem slashNormal02;
    [SerializeField] private ParticleSystem slashNormal03;   
    [SerializeField] private ParticleSystem runeSlash;
 
    public void Play01()
    {
        slashNormal01.Play();
    }
    public void Play02()
    {
        slashNormal02.Play();
    }
    public void Play03()
    {
        slashNormal03.Play();
    }
    public void RuneSlash()
    {
        runeSlash.Play();
    }
}

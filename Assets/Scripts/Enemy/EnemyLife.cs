using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLife : MonoBehaviour
{
    private bool isDied = false; 
    [Header("Player Health")]
    public float health;
    private float maxHealth;


    [Header("UI Elements")]
    public Image frontHealth;
    public Image backHealth;
    public Image baseHealth;
    public float lerpTimer; 
    public float chipSpeed = 2f;
    private Color32 DamageHealthColor = new Color32 (128, 128, 0, 255);
    
    public static bool isAlive = true;



    private Animator anim;
    


    public void Awake()
    {
        isAlive = true;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        health = 200f;
        maxHealth = 200f;
        HealthBarColor();
        UpdateHealthUI();
    }

    private void DeathAnimation()
    {
        isDied = true; 
        isAlive = false;
        isAlive = true;
        isDied = false;
    }

    public void EnemyDie()
    {
        Time.timeScale = 0f;
    }
    
    private void HealthBarColor()
    {
        if (health <= maxHealth && health >= maxHealth * 0.6f) 
        {
            frontHealth.color = GetColorFromString("FF2613");
        }
        
        if (health <= maxHealth * 0.5f && health >= maxHealth * 0.3f) 
        {
            frontHealth.color = GetColorFromString("FF2613");
        }
        
        if (health <= maxHealth * 0.2f && health >= 0f) 
        {
            frontHealth.color = GetColorFromString("FF2613");
        }
    }
    public void UpdateHealthUI()
    {
        float fillF = frontHealth.fillAmount;
        float fillB = backHealth.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealth.fillAmount = hFraction;
            backHealth.color = DamageHealthColor;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; 
            backHealth.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealth.color = Color.white;
            backHealth.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; 
            frontHealth.fillAmount = Mathf.Lerp(fillF, backHealth.fillAmount, percentComplete);
        }
    }

    public static Color GetColorFromString(string color) 
    {
        float red = Hex_to_Dec01(color.Substring(0,2));
        float green = Hex_to_Dec01(color.Substring(2,2));
        float blue = Hex_to_Dec01(color.Substring(4,2));
        float alpha = 1f;
        if (color.Length >= 8) {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6,2));
        }
        return new Color(red, green, blue, alpha);
    }
    public static int Hex_to_Dec(string hex) 
    {
        return Convert.ToInt32(hex, 16);
    }
    public static float Hex_to_Dec01(string hex) 
    {
        return Hex_to_Dec(hex)/255f;
    }
    public void OffBar()
    {
        baseHealth.enabled = false;
        backHealth.enabled = false;
        frontHealth.enabled = false;
    }
    public void OnBar()
    {
        baseHealth.enabled = true;
        backHealth.enabled = true;
        frontHealth.enabled = true;
    }
}

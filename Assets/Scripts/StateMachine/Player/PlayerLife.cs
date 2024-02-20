using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Manages the player's life UI.
/// </summary>
public class PlayerLife : MonoBehaviour
{
    private bool isDied = false; 
    [Header("Player Health")]
    public float health;
    [SerializeField] private float maxHealth;


    [Header("UI Elements")]
    public Image frontHealth;
    public Image backHealth;
    public float lerpTimer; 
    public float chipSpeed = 2f;
    private Color32 DamageHealthColor = new Color32 (219, 49, 49, 255);
    
    //Flags
    public static bool isAlive = true;


    //References
    private Animator anim;
    
    //Singleton
    public static PlayerLife Instance { get; private set; }
    


    public void Awake()
    {
        isAlive = true;
        Instance = this;
        anim = GetComponent<Animator>();
    }
    private void Start() 
    {
        maxHealth = 200;
        health = GetComponent<Health>().health;
    }

    void Update()
    {
        health = GetComponent<Health>().health;
        UpdateHealthUI();
        if (health <= 0 && !isDied)
        {
            Death();
        }
    }

    /// <summary>
    /// Initiates the death method player.
    /// </summary>
    private void Death()
    {
        isDied = true; 
        isAlive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the health UI based on the player's current health.
    /// </summary>
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


    /// <summary>
    /// Converts a hex color string to a Color object.
    /// </summary>
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

    /// <summary>
    /// Converts a hex string to a decimal value.
    /// </summary>
    public static int Hex_to_Dec(string hex) 
    {
        return Convert.ToInt32(hex, 16);
    }

    /// <summary>
    /// Converts a hex string to a decimal value between 0 and 1.
    /// </summary>
    public static float Hex_to_Dec01(string hex) 
    {
        return Hex_to_Dec(hex)/255f;
    }
}

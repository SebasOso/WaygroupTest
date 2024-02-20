
using UnityEngine;


/// <summary>
/// Manages the player's armor and equipment.
/// </summary>
public class ArmorManager : MonoBehaviour
{
    public static ArmorManager Instance { get; private set; }  

    //Variables to calculate the armor
    private float totalArmor;
    private float shoulderArmor;

    //Player reference
    private Health playerHealth;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        playerHealth = GetComponent<Health>();
        CalculateTotalArmor();
    }

    /// <summary>
    /// Equips shoulder armor to the player.
    /// </summary>
    public void EquipShoulder(Shoulder shoulderToEquip)
    {
        ShoulderArmorManager.Instance.SetShoulders(shoulderToEquip);
        shoulderArmor = shoulderToEquip.GetArmor();
        CalculateTotalArmor();
        ArmorDisplay.Instance.SetArmor();
    }

    /// <summary>
    /// Calculates the total armor value based on equipped armor pieces.
    /// </summary>
    private void CalculateTotalArmor()
    {
        if(ShoulderArmorManager.Instance.shoulder != null)
        {
            shoulderArmor = ShoulderArmorManager.Instance.shoulder.GetArmor();
        }
        totalArmor = shoulderArmor;
        playerHealth.armor = totalArmor;
    }

    /// <summary>
    /// Retrieves the total armor value of the player.
    /// </summary>
    /// <returns>Total armor value</returns>
    public float GetArmor()
    {
        return playerHealth.armor;
    }
}

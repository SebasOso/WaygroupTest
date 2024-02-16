using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    public static ArmorManager Instance { get; private set; }  

    public float totalArmor;
    public float shoulderArmor;

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
    public void EquipShoulder(Shoulder shoulderToEquip)
    {
        ShoulderArmorManager.Instance.SetShoulders(shoulderToEquip);
        shoulderArmor = shoulderToEquip.GetArmor();
        CalculateTotalArmor();
        ArmorDisplay.Instance.SetArmor();
    }
    private void CalculateTotalArmor()
    {
        if(ShoulderArmorManager.Instance.shoulder != null)
        {
            shoulderArmor = ShoulderArmorManager.Instance.shoulder.GetArmor();
        }
        totalArmor = shoulderArmor;
        playerHealth.armor = totalArmor;
    }
    public float GetArmor()
    {
        return playerHealth.armor;
    }
}

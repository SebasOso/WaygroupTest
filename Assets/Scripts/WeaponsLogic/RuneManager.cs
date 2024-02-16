using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneManager : MonoBehaviour
{
    public static RuneManager Instance { get; private set; }
    public bool isCoolDown;
    [SerializeField] public float coolDown = 15f;
    [SerializeField] public float abilityCoolDown = 15f;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        if(GetComponent<Armory>().currentWeapon.value.CanRuneAttack == false)
        {
            return;
        }
        abilityCoolDown = GetComponent<Armory>().currentWeapon.value.coolDown;
        coolDown = abilityCoolDown;
        isCoolDown = false;
        GetComponent<Armory>().abilityImage.fillAmount = 1;
    }
    void Update()
    {
        if(GetComponent<Armory>().currentWeapon.value.CanRuneAttack == false)
        {
            return;
        }
        AbilityBehaviour();
    }
    public void Ability()
    {
        GetComponent<Armory>().abilityImage.fillAmount = 0;
        isCoolDown = true;
    }
    public void UpdateAbility()
    {
        abilityCoolDown = GetComponent<Armory>().currentWeapon.value.coolDown;
        coolDown = abilityCoolDown;
        isCoolDown = true;
        GetComponent<Armory>().abilityImage.fillAmount = 0;
    }
    private void AbilityBehaviour()
    {
        if (isCoolDown)
        {
            float timeRemaining = coolDown;  
            if (timeRemaining > 0)
            {
                float fillAmount = Mathf.Clamp(1 - (timeRemaining / GetComponent<Armory>().currentWeapon.value.coolDown), 0f, 1f);
                GetComponent<Armory>().abilityImage.fillAmount = fillAmount;
            }
            else
            {
                GetComponent<Armory>().abilityImage.fillAmount = 1;
                coolDown = GetComponent<Armory>().currentWeapon.value.coolDown;
                isCoolDown = false;
            }
            coolDown -= Time.deltaTime;
        }
    }
}

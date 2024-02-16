using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using RPG.Combat;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Rendering;

public class Armory : MonoBehaviour, IJsonSaveable, IModifierProvider
{
    public static Armory Instance;
    [Header("Weapons")]
    [SerializeField] public Weapon defaultWeapon;
    [SerializeField] private Transform rightHandSocket;
    [SerializeField] private Transform leftHandSocket;
    [SerializeField] private Transform backSocket;
    [SerializeField] private Transform backWeapon;
    public WeaponPickup weaponToPickUp;
    public float damage = 0f;

    [Header("Sounds")]
    [SerializeField] private AudioSource weaponSoundsSource;
    [SerializeField] private AudioClip bowShoot;
    [SerializeField] private AudioClip bowLoad;
    [SerializeField] private AudioClip freezeArrow;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    public LazyValue<Weapon> currentWeapon;
    public Weapon disarmedWeapon;

    [SerializeField] Targeter Targeter;

    [Header("Abilities")]
    private float coolDown = 15f;
    private float abilityCoolDown = 15f;
    private bool isCoolDown = false;
    [SerializeField] public Image abilityImage;
    [SerializeField] public Image abilityBackGround;

    private void Awake() 
    {
        currentWeapon = new LazyValue<Weapon>(GetInitialWeapon);
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private Weapon GetInitialWeapon()
    {
        AttachWeapon(defaultWeapon);
        return defaultWeapon;
    }
    private void Start() 
    {
        currentWeapon.ForceInit();
        animator.SetFloat("attackSpeed", GetComponent<BaseStats>().GetStat(Stat.AttackSpeed));
        damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        if(currentWeapon.value.CanRuneAttack == true)
        {
            coolDown = abilityCoolDown;
            isCoolDown = false;
            abilityImage.fillAmount = 1;
            abilityImage.sprite = currentWeapon.value.runeAttackImage;
            abilityBackGround.sprite = currentWeapon.value.runeAttackImage;
            return;
        }
    }
    private void OnEnable() 
    {
        GetComponent<BaseStats>().OnLevelUP += UpdateAS;
        GetComponent<BaseStats>().OnLevelUP += UpdateDamage;
    }
    private void OnDisable() 
    {
        GetComponent<BaseStats>().OnLevelUP -= UpdateAS;
        GetComponent<BaseStats>().OnLevelUP -= UpdateDamage;
    }
    private void UpdateDamage()
    {
        damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
    }

    private void UpdateAS()
    {
        animator.SetFloat("attackSpeed", GetComponent<BaseStats>().GetStat(Stat.AttackSpeed));
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon.value = weapon;
        AttachWeapon(weapon);
        damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        UpdateAbility();
    }
    public void EquipDisarmedWeapon()
    {
        if(disarmedWeapon == null)return;
        backWeapon.gameObject.SetActive(false);
        Debug.Log("EQUIPING WEAPON");
        EquipWeapon(disarmedWeapon);
        disarmedWeapon = defaultWeapon;
        //EQUIP THE DISARMED WEAPON, SET THE DISARMED WEAPON TO DEFAULT WEAPON (UNNARMED)
    }
    public void DesactivateBackWeapon()
    {
        if(backWeapon == null){return;}
        backWeapon?.gameObject.SetActive(false);
    }
    public void UnequipWeapon()
    {
        Debug.Log("UNEQUIPING WEAPON");
        AttachWeaponBack(currentWeapon.value);
        backWeapon = backSocket.Find("Weapon");
        disarmedWeapon = currentWeapon.value;
        //ATTACH WEAPON TO BACK, AND SET THE DISARMED WEAPON TO THE CURRENT WEAPON
        EquipWeapon(defaultWeapon);
    }
    private void AttachWeaponBack(Weapon weapon)
    {
        weapon.SpawnBack(backSocket);
    }
    private void AttachWeapon(Weapon weapon)
    {
        weapon.Spawn(rightHandSocket, leftHandSocket, animator);
        GetComponent<InputReader>().CanRuneAttack = weapon.CanRuneAttack;
        GetComponent<InputReader>().CanDisarm = weapon.CanDisarm;
        GetComponent<InputReader>().IsEquipped = weapon.IsEquipped;
    }

    public void PickUpWeapon()
    {
        weaponToPickUp?.PickUp();
        GetComponent<InputReader>().IsInteracting = false;
        Destroy(weaponToPickUp.gameObject);
    }
    private void UpdateAbility()
    {
        RuneManager.Instance.UpdateAbility();  
        abilityImage.sprite = currentWeapon.value.runeAttackImage;
        abilityBackGround.sprite = currentWeapon.value.runeAttackImage;
    }
    public JToken CaptureAsJToken()
    {
        if(currentWeapon.value.name == defaultWeapon.name)
        {
            if(disarmedWeapon == null)
            {
                return JToken.FromObject(defaultWeapon.name);
            }
            return JToken.FromObject(disarmedWeapon.name);
        }
        return JToken.FromObject(currentWeapon.value.name);
    }
    void Shoot()
    {
        if (Targeter.currentTarget == null || Targeter.currentTarget.GetComponent<Health>().IsDead()) return;
        if(currentWeapon.value.HasProjectile())
        {
            currentWeapon.value.LaunchProjectile(rightHandSocket,leftHandSocket,Targeter.currentTarget.GetComponent<Health>(), damage);
        }
    }
    public void FreezeShoot()
    {
        if (Targeter.currentTarget == null || Targeter.currentTarget.GetComponent<Health>().IsDead()) return;
        if (currentWeapon.value.HasProjectile())
        {
            print("FREEEEEEEEZE");
            currentWeapon.value.LaunchFreezeArrow(rightHandSocket, leftHandSocket, Targeter.currentTarget.GetComponent<Health>(), damage + 10f);
            PlayFreeze();
        }
    }
    public void PlayFreeze()
    {
        weaponSoundsSource.clip = freezeArrow;
        weaponSoundsSource.Play();
    }
    public void PlayShoot()
    {
        weaponSoundsSource.clip = bowShoot;
        weaponSoundsSource.Play();
    }
    public void PlayLoad()
    {
        weaponSoundsSource.clip = bowLoad;
        weaponSoundsSource.Play();
    }
    public void RestoreFromJToken(JToken state)
    {
        string weaponName = state.ToObject<string>();
        Weapon weapon = Resources.Load<Weapon>(weaponName);
        EquipWeapon(weapon);
    }

    public IEnumerable<float> GetAdditiveModifier(Stat stat)
    {
        if(stat == Stat.Damage)
        {
            yield return currentWeapon.value.GetWeaponDamage();
        }
    }

    public IEnumerable<float> GetPercentageModifier(Stat stat)
    {
        if(stat == Stat.Damage)
        {
            yield return currentWeapon.value.GetPercentageDamage();
        }
    }
    
}

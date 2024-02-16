using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Combat;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

public class EnemyArmory : MonoBehaviour, IJsonSaveable, IModifierProvider
{
    [Header("Weapons")]
    [SerializeField] public Weapon defaultWeapon = null;
    [SerializeField] private Transform rightHandSocket;
    [SerializeField] private Transform leftHandSocket;

    [Header("Sounds")]
    [SerializeField] private AudioSource weaponSoundsSource;
    [SerializeField] private AudioClip bowShoot;
    [SerializeField] private AudioClip bowLoad;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    public LazyValue<Weapon> currentWeapon;
    public float damage;
    Health Player; 
    private void Awake() 
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Health>();
        currentWeapon = new LazyValue<Weapon>(GetInitialWeapon);
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
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon.value = weapon;
        AttachWeapon(weapon);
    }

    private void AttachWeapon(Weapon weapon)
    {
        weapon.Spawn(rightHandSocket, leftHandSocket, animator);
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentWeapon.value.name);
    }
    void Shoot()
    {
        if (Player == null || Player.IsDead()) return;
        if(currentWeapon.value.HasProjectile())
        {
            currentWeapon.value.LaunchProjectile(rightHandSocket,leftHandSocket,Player, damage);
        }
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

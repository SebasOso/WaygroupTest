
using RPG.Combat;
using UnityEngine;

public class EnemyArmory : MonoBehaviour
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
    public Weapon currentWeapon;
    public float damage;
    Health Player; 
    private void Awake() 
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Health>();
        currentWeapon = GetInitialWeapon();
    }
    private Weapon GetInitialWeapon()
    {
        AttachWeapon(defaultWeapon);
        return defaultWeapon;
    }
    private void Start() 
    {
        animator.SetFloat("attackSpeed", 1.5f);
        damage = 20f;
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        AttachWeapon(weapon);
    }

    private void AttachWeapon(Weapon weapon)
    {
        weapon.Spawn(rightHandSocket, leftHandSocket, animator);
    }
    void Shoot()
    {
        if (Player == null || Player.IsDead()) return;
        if(currentWeapon.HasProjectile())
        {
            currentWeapon.LaunchProjectile(rightHandSocket,leftHandSocket,Player, damage);
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
}


using UnityEngine;
using UnityEngine.UI;

public class HealManager : MonoBehaviour
{
    public static HealManager Instance { get; private set; }

    [Header("Ability")]
    public bool isCoolDown;
    [SerializeField] int numberOfHits = 6;
    [SerializeField] int actualHits = 6;

    [Header("UI")]
    [SerializeField] Image healImage;
    [SerializeField] Image healBackground;
    [SerializeField] AudioSource audioSource;

    [Header("Audio")]
    [SerializeField] AudioClip healClip;

    private InputReader inputReader;
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
        inputReader = GetComponent<InputReader>();
        playerHealth = GetComponent<Health>();
        isCoolDown = false;
        healImage.fillAmount = 1;
    }
    void Update()
    {
        inputReader.CanHeal = actualHits != numberOfHits ? false : (!isCoolDown);
    }
    public void Ability()
    {
        actualHits = 0;
        audioSource.clip = healClip;
        audioSource.Play();
        healImage.fillAmount = 0;
        isCoolDown = true;
    }
    public void AddHitHeal()
    {
        if(isCoolDown)
        {
            actualHits++;
            healImage.fillAmount = (float)actualHits / numberOfHits;
            if(actualHits >= numberOfHits)
            {
                healImage.fillAmount = 1;
                isCoolDown = false;
            }
        }
    }
}

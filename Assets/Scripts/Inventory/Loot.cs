using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "RPGCombatSystem/Loot", order = 0)]
public class Loot : ScriptableObject 
{
    public GameObject prefab;
    public int dropChance;
    public string lootName;
    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
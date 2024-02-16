using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Header("Stats")]
        [Range(1, 50)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        public event Action OnLevelUP;
        LazyValue<int> currentLevel;
        Experience experience;
        private void Awake() 
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start() 
        {
            currentLevel.ForceInit();
        }
        private void OnEnable() 
        {
            if(experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable() 
        {
            if(experience != null)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }
        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUPEffect();
                OnLevelUP();
            }
        }

        private void LevelUPEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }
        public float GetBaseStat(Stat stat, int level)
        {
            return progression.GetStat(stat, characterClass, GetLevel() - level);
        }
        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }
        public int GetLevel()
        {
            if(currentLevel.value == 1)
            {
                currentLevel.value = CalculateLevel();
            }
            return currentLevel.value;
        }
        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(experience == null) return startingLevel;
            float currentXP = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}

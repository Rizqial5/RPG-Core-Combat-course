using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticle = null;
        [SerializeField] bool shouldUseModifiers = false;

        LazyValue<int> currentLevel;
        public event Action onLevelUp;

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
                experience.onExpereienceGained += UpdateLevel; //store update level method to Delegate onExperienceGained
            }
        }

        private void OnDisable() 
        {
            
            if(experience != null)
            {
                experience.onExpereienceGained -= UpdateLevel; //store update level method to Delegate onExperienceGained
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                print("something");
                LevelUpEffect();
                onLevelUp();
            }

        }
        
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifiers(stat)/100);
        }

        

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStats(stat, characterClass, GetLevel());
        }


        public int GetLevel()
        {
            return currentLevel.value;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if(experience == null) return startingLevel;


            float currentXP = experience.GetExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int levels = 1; levels <= penultimateLevel; levels++)
            {
                float XPtoLevelUp = progression.GetStats(Stat.ExperienceToLevelUp, characterClass, levels);
                if (XPtoLevelUp > currentXP)
                {
                    return levels;
                }
                
            }
            return penultimateLevel + 1;
            
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            if(!shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticle, transform);    
        }

        
    }
}

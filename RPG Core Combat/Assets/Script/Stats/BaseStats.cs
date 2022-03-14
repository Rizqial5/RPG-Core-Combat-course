using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

        private void Start() 
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.onExpereienceGained += UpdateLevel; //store update level method to Delegate onExperienceGained
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                print("something");
            }

        }
        
        public float GetStat(Stat stat)
        {
            return progression.GetStats(stat,characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
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

        
    }
}

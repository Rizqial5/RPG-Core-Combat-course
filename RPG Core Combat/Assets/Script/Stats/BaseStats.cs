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


        private void Update() 
        {
            if(gameObject.tag == "Player")
            {
                print(GetLevel());
            }    
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStats(stat,characterClass, GetLevel());
        }

        public int GetLevel()
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

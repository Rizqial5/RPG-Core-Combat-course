using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{


    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject 
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStats(Stat stat,CharacterClass characterClass, int level)
        {

            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];

            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat,CharacterClass characterClass)
        {
            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if(lookUpTable != null) return;
            
            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionCharacter in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();
                
                foreach(ProgressionStat progressionStat  in progressionCharacter.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                lookUpTable[progressionCharacter.characterClass] = statLookUpTable;
            }

        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
        
    }
}
using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable 
    {
        float health = -1f;
        bool isDeath = false;

        private void Start() 
        {
            if(health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
           
        }

        public bool isDead()
        {
            return isDeath;
        }

        public float GetPercentage()
        {
            return 100 * (health / GetComponent<BaseStats>().GetStat(Stat.Health)) ;
        }

        
        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage,0);

            if(health == 0 )
            {
                Die();
                AwardExperience(instigator);
            }
        }

        

        private void Die()
        {
            if(isDeath) return;
            isDeath = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
            
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            
            health = (float) state;
            if(health <= 0)
            {
                Die();
            }
        }


    }
}
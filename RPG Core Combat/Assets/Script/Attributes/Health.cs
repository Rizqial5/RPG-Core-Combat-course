using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable 
    {

        bool isDeath = false;

        [SerializeField] float regenPercentage = 70;
        [SerializeField] UnityEvent takeDamege = null;

        LazyValue<float> healthPoints;
        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitalHealth);
        }

        private float GetInitalHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start() 
        {
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerationHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerationHealth;
        }

        

        public bool isDead()
        {
            return isDeath;
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health)) ;
        }

        
        public void TakeDamage(GameObject instigator, float damage)
        {
            // print(gameObject.name + " took damage : " + damage);
            
            healthPoints.value = Mathf.Max(healthPoints.value - damage,0);

            if(healthPoints.value == 0 )
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamege.Invoke();
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float MaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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

        private void RegenerationHealth()
        {
            float regenHealth = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenPercentage/100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealth);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            
            healthPoints.value = (float) state;
            if(healthPoints.value <= 0)
            {
                Die();
            }
        }


    }
}
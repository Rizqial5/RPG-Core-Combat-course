using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour 
    { 
        Health health;
        Text text;
        Fighter fighter;

        private void Awake() 
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        private void Update() 
        {
            if(fighter.GetTarget() == null)
            {
                text.text = "N/A";
                return;
                
            }
            Health health = fighter.GetTarget();

            text.text = String.Format( "{0:0}/{1:0}",health.GetHealthPoints(), health.MaxHealthPoints());
        }
    }
}
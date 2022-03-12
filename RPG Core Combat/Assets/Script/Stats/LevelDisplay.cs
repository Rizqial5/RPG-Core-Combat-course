using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour 
    {
        BaseStats level;
        Text text;

        private void Awake() 
        {
            level = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            text = GetComponent<Text>();
        }

        private void Update() 
        {
            text.text = String.Format( "{0}",level.GetLevel());
        }
    }
}

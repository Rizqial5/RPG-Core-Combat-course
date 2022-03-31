using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamagedText
{
    public class DamagedTextSpawner : MonoBehaviour
    {
       
        [SerializeField] DamagedText damagedTextPrefab = null;
        
        public void Spawn(float damageAmount)
        {
            
            DamagedText instance = Instantiate<DamagedText>(damagedTextPrefab,transform);
        }
    }
}

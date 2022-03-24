using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {   
        [SerializeField] GameObject target = null;
        void Update() 
        {
            if(!GetComponent<ParticleSystem>().IsAlive())
            {
                if(target != null)
                {
                    Destroy(target);
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }    

        }
    }
}

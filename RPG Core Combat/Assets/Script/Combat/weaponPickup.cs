using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class weaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        Fighter fighter ;

        private void Start() {
            
            
        }
        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
                print("bisa");
                Destroy(gameObject);
            }

        }



    }   
}

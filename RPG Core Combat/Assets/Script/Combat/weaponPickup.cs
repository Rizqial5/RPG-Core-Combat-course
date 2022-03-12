using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class weaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5;
        
        Fighter fighter ;



        private void Start() {
            
            
        }
        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
                
            }

        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool isShow)
        {
            GetComponent<SphereCollider>().enabled = isShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isShow);
            }
        }

        
    }   
}

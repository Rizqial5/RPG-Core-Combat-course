using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class weaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float respawnTime = 5;
        [SerializeField] float restoreHealth = 0;
        
        Fighter fighter ;

        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);

            }

        }

        private void Pickup(GameObject subject)
        {
            if(weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if(restoreHealth > 0)
            {
                subject.GetComponent<Health>().Heal(restoreHealth);
            }
            
            StartCoroutine(HideForSeconds(respawnTime));
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

        public bool HandleRaycast(PlayerController callingControl)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Pickup(callingControl.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }   
}

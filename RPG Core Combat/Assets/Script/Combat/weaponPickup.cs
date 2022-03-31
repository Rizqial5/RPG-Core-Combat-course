using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class weaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5;
        
        Fighter fighter ;



        
        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());

            }

        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
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
                Pickup(callingControl.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }   
}

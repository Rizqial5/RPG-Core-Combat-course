using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {


        
        [SerializeField] float speed = 2f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destoryOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        Health target = null;
        float damage = 0f;
        

        private void Start() 
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {  
            if(target == null) return;
            if(isHoming && !target.isDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate( Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();
            if(capsuleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            
            if(other.GetComponent<Health>() != target) return;
            if(target.isDead()) return;
            target.TakeDamage(damage);

            speed = 0;
            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destoryOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
            
            
            
            
        }
    }
}

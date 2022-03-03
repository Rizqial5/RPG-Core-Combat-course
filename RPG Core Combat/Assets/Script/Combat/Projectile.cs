using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {


        [SerializeField] Transform target = null;
        [SerializeField] float speed = 2f;
        
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {  
            if(target == null) return;
            transform.LookAt(GetAimLocation());
            transform.Translate( Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();
            if(capsuleCollider == null)
            {
                return target.position;
            }
            return target.position + Vector3.up * capsuleCollider.height / 2;
        }
    }
}

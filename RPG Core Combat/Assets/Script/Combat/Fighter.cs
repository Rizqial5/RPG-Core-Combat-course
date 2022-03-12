using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
       
        
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Mover mover;
        Weapon currentWeapon = null;
        private void Start() {
            mover = GetComponent<Mover>();

            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon); 
            }
                      
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(animator, rightHandTransform, leftHandTransform);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead()) return;

            if (!GetInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
                

            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                //This will trigger Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;


            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if(target == null) return;

            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform,leftHandTransform,target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, currentWeapon.GetWeaponDamage());
            }

            
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject  combattarget)
        {
            if(combattarget == null) {return false; }
            Health targetToTest = combattarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.isDead();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);

        }
    }
}
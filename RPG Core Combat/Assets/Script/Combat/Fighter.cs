using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
       
        
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Mover mover;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake() {
            mover = GetComponent<Mover>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start() {
            

            currentWeapon.ForceInit();
                      
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(animator, rightHandTransform, leftHandTransform);
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

            if (!GetInRange(target.transform))
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            
            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if(currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform,leftHandTransform,target, gameObject, damage);
            }
            else
            {
                
                target.TakeDamage(gameObject, damage);
            }

            
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetInRange( Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        public bool CanAttack(GameObject  combattarget)
        {
            if(combattarget == null) {return false; }
            if(!GetComponent<Mover>().CanMoveTo(combattarget.transform.position) &&
            !GetInRange(combattarget.transform)) 
            { 
                return false;
            }
            Health targetToTest = combattarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.isDead();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);

        }

        
    }
}
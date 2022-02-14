using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damageWeapon = 5f;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Mover mover;
        private void Start() {
            mover = GetComponent<Mover>();            
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead()) return;

            if (!GetInRange())
            {
                mover.MoveTo(target.transform.position);
                

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
            target.TakeDamage(damageWeapon);
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
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


    }
}
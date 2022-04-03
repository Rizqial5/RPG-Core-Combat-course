using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null ;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        

        const string weaponName = "Weapon";

        public void Spawn( Animator animator, Transform rightHand, Transform leftHand)
        {

            DestroyOldWeapon(rightHand,leftHand);
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if(weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(weaponPrefab, handTransform);
                weapon.name = weaponName;
            }

            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;   
            }
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;

            oldWeapon.name = "DESTROYY";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damageCalculated)
        {
            Projectile projectileInstance = Instantiate(projectile,GetTransform(rightHand, leftHand).position, Quaternion.identity);
            //call
            projectileInstance.SetTarget(target, instigator,damageCalculated);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}

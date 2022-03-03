using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null ;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;

        public void Spawn( Animator animator, Transform rightHand, Transform leftHand)
        {
            if(weaponPrefab != null)
            {
                Transform handTransform;
                if(isRightHanded) handTransform = rightHand;
                else handTransform = leftHand;
                Instantiate(weaponPrefab, handTransform);
            }
            
            if(weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
            
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}

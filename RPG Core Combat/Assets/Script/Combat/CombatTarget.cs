using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    // [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingControl)
        {
           
            if(!callingControl.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            } 

            if(Input.GetMouseButton(0))
            {
                callingControl.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
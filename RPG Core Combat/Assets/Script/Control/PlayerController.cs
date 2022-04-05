using RPG.Movement;
using RPG.Combat;
using UnityEngine;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
{
    Health health;

    [System.Serializable]
    struct CursorMapping
    {
        
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;

    }

    [SerializeField]  CursorMapping[] cursorMappings = null;
    [SerializeField] float maxNavMeshDistance = 1f;
    [SerializeField] float rayRadius = 1f;
    


    

    private void Awake() {
        health = GetComponent<Health>();
    }
    
    private void Update()
        {
            if(InteractWithUI()) return;
            if(health.isDead()) 
            {
                SetCursor(CursorType.Dead);
                return;
            }
            
            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);

        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            //Get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), rayRadius);
            //build array distance
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            //sort the hits
            Array.Sort(distances, hits);
            //return
            return hits ;
        }

        private bool InteractWithUI()
        {
           if(EventSystem.current.IsPointerOverGameObject())
           {
               SetCursor(CursorType.UI);
               return true;
           }

           return false;
        }

        

        private bool InteractWithMovement()
        {

            
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);

            if (hasHit)
            {

                if(!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;


        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) return false;

            //Find nearest navmesh point
            NavMeshHit navMeshHit;
            bool hasNavMeshHit = NavMesh.SamplePosition(hit.point,out navMeshHit,maxNavMeshDistance, NavMesh.AllAreas);
            if(!hasNavMeshHit) return false;

            target = navMeshHit.position;
            


            
            return true;
        }

        

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
                
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
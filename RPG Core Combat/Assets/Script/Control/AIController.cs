using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float distance = 5f;
        [SerializeField] float suspiciusTime = 3f;
        [SerializeField] float dwellTime = 3f;
        [SerializeField] float aggroTime = 3f;
        [SerializeField] float shoutRadius = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolFractionSpeed = 0.2f;
        

        Fighter fighter;
        GameObject player;
        Health health;
        LazyValue<Vector3> guardPosition;
        Mover mover;

        
        float timeSinceSawPlayer = Mathf.Infinity;
        float timeSinceAtWaypoint = Mathf.Infinity;
        float timeAgrro = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetInitialPosition);
        }

        private Vector3 GetInitialPosition()
        {
            return  transform.position;
        }

        private void Start() {
            guardPosition.ForceInit();
        }
        private void Update()
        {
            if (health.isDead()) return;

            if (IsAggrevate() && fighter.CanAttack(player))
            {
                
                AttackBehaviour();
            }
            else if (timeSinceSawPlayer < suspiciusTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                fighter.Cancel();
                PatrolBehaviour();

            }
            UpdateTimers();

        }

        public void Aggrevate()
        {
            timeAgrro = 0;
        }

        private void UpdateTimers()
        {
            timeSinceSawPlayer += Time.deltaTime;
            timeSinceAtWaypoint += Time.deltaTime;
            timeAgrro += Time.deltaTime; 
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    timeSinceAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceAtWaypoint>dwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolFractionSpeed);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
           return patrolPath.GetWaypoints(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.NextWaypointsIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceWaypoint = Vector3.Distance(transform.position,GetCurrentWaypoint());
            return distanceWaypoint < waypointTolerance;
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutRadius, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
               AIController ai = hit.collider.GetComponent<AIController>();

               if(ai == null) continue;

               ai.Aggrevate();
            }
        }

        private bool IsAggrevate()
        {
            float DistancePlayer = Vector3.Distance(player.transform.position, transform.position);
            
            return DistancePlayer < distance || timeAgrro < aggroTime;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, distance);
        }

      
    }
}
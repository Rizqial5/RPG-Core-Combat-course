using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour 
    {
        [SerializeField] float distance = 5f;
        [SerializeField] float suspiciusTime = 3f;
        [SerializeField] float dwellTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolFractionSpeed = 0.2f;
        

        Fighter fighter;
        GameObject player;
        Health health;
        Vector3 guardPosition;
        Mover mover;

        
        float timeSinceSawPlayer = Mathf.Infinity;
        float timeSinceAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start() {
            
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();
            

            guardPosition = transform.position;
            

        }
        private void Update()
        {
            if (health.isDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
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

        private void UpdateTimers()
        {
            timeSinceSawPlayer += Time.deltaTime;
            timeSinceAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
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
        }

        private bool InAttackRangeOfPlayer()
        {
            float DistancePlayer = Vector3.Distance(player.transform.position, transform.position);
            return DistancePlayer < distance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, distance);
        }

      
    }
}
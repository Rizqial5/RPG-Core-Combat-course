using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement 
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        Ray lastRay;
        Health health;
        

        NavMeshAgent navMeshAgent;
        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

    
        void Update()
        {
            navMeshAgent.enabled = !health.isDead();
            UpdateAnimator();    
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            
            MoveTo(destination, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
            
            
            
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        
        
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("moveForward", speed);
        }

        public object CaptureState()
        {
            return new SerializeableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializeableVector3 position = (SerializeableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

}
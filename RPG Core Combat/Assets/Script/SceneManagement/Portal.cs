using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum PortalIdentifier
        {
            A, B, C, D
        }
        [SerializeField] int StageScene;
        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalIdentifier destination;

        [SerializeField] float FadeInTime;
        [SerializeField] float FadeOutTime;
        [SerializeField] float BetweenFadeTime;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
                
            }
        }

        private IEnumerator Transition()
        {


            Fade fade = FindObjectOfType<Fade>();


            DontDestroyOnLoad(gameObject);
            yield return fade.FadeOut(FadeOutTime);
            yield return SceneManager.LoadSceneAsync(StageScene);
            
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(BetweenFadeTime);
            yield return fade.FadeIn(FadeInTime);

            Destroy(gameObject);
            
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;
                
                return portal;
            }
            return null;
        }
    }

}
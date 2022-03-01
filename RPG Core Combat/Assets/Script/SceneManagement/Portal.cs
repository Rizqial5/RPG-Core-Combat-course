using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

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

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            DontDestroyOnLoad(gameObject);
            yield return fade.FadeOut(FadeOutTime);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(StageScene);
            savingWrapper.Load();
            
            
            Portal otherPortal = GetOtherPortal();
            
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
           
            yield return new WaitForSeconds(BetweenFadeTime);
            yield return fade.FadeIn(FadeInTime);

            Destroy(gameObject);
            
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
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
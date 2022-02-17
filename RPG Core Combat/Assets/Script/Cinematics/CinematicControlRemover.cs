using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;
using RPG.Core;


namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        
        private void Start() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        public void DisableControl(PlayableDirector dummy)
        {
            
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public void EnableControl(PlayableDirector dummy)
        {
            
            player.GetComponent<PlayerController>().enabled = true;
        }



    }
}

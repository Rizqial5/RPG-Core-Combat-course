using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int StageScene;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(StageScene);
            }
        }
    }

}
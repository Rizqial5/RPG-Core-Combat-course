using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fade : MonoBehaviour
    {

        CanvasGroup canvasGroup;

        // Start is called before the first frame update
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) //alpha is not 1
            {
                canvasGroup.alpha += Time.deltaTime/time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) //alpha is  1
            {
                canvasGroup.alpha -= Time.deltaTime/time;
                yield return null;
            }
        }


    }
    
}
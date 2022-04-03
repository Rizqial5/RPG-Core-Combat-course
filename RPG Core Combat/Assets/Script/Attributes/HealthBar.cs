using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
        void Update()
        {
            if(Mathf.Approximately(healthComponent.GetFraction(),0) || 
            Mathf.Approximately(healthComponent.GetFraction(),1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetFraction(),1,1);
            // healthComponent.GetFraction();
            

        }

        // private void DestroyHealth()
        // {
        //     if(foreground.localScale.x == 0)
        //     {
        //         Destroy(gameObject);
        //     }
        // }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamagedText
{
        public class DamagedText : MonoBehaviour
        {
            [SerializeField] Text damagedText = null;

            public void SetValue(float amount)
            {
                damagedText.text = String.Format( "{0:0}",amount);
            }
        }
}

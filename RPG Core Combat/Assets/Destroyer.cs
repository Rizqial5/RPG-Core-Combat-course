using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject toDesroy = null;

    public void DestroyTarget()
    {
        Destroy(toDesroy);
    }
}

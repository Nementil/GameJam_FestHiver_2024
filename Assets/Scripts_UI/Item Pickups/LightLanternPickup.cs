using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLanternPickup : MonoBehaviour
{
    [SerializeField] private float amountLight;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerLantern>().RecoverLight(amountLight);
            Destroy(gameObject);    
        }
    }
}

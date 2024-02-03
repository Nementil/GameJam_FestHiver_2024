using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerCollectibleHolder>() != null)
        {
            other.GetComponent<PlayerCollectibleHolder>().CollectibleCounter(1);
            gameObject.SetActive(false);
        }
    }
}

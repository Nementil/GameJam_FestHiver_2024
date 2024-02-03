using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Header("Win Screen")]
    [SerializeField] private GameObject winScreen;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(winScreen != null)
            {
                winScreen.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}

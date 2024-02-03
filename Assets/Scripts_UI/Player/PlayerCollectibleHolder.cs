using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibleHolder : MonoBehaviour
{

    [Header("Key Attributes")]
    private float startingAmountKey = 0; // initial amount of key
    public float currentAmountKey { get; set; }
    [SerializeField] private float maxAmountKey = 3; // initial amount of key


    [Header("Win Screen")]
    [SerializeField] private GameObject winScreen;



    void Awake()
    {
        currentAmountKey = startingAmountKey;
    }

    void Update()
    {

    }


    public void CollectibleCounter(float singleKey)
    {

        currentAmountKey = Mathf.Clamp(currentAmountKey + singleKey, startingAmountKey, maxAmountKey);

        if (currentAmountKey == maxAmountKey)
        {
            // Win UI
            if (winScreen != null)
            {
                winScreen.SetActive(true);
            }
        }

        UIController.instance.UpdateCollectibleHolder(currentAmountKey / 10, maxAmountKey / 10);

    }

}

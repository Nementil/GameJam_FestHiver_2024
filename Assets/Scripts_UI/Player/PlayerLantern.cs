using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    [Header("Light Duration (Seconds)")]
    [SerializeField] private float startingLight; // Value light in seconds
    public float currentLight { get;set; }

    private bool isLanternActive = false;
    private float clickTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        currentLight = startingLight;
    }


    void Update()
    {

        // Enable use of Lantern
        if (Input.GetMouseButtonDown(0))
        {
            LanternActivate();
        }

        // Lantern in usage
        LanternState();


        // Disable use of Lantern
        if (Input.GetMouseButtonUp(0))
        {
            LanternDeactivate();
        }

        // Update UI 
        currentLight = Mathf.Clamp(currentLight, 0, startingLight);

        UIController.instance.UpdateLantern(currentLight, startingLight);
    }

    private void LanternDeactivate()
    {
        isLanternActive = false;
    }

    void LanternState()
    {
        if (isLanternActive)
        {
            clickTime += Time.deltaTime;
            currentLight -= Time.deltaTime;


            if (currentLight >= 0)
            {
                Debug.Log("Temps de clic : " + clickTime);
            }
            else
            {
                Debug.Log("Out of Light !");
            }
        }
    }

    void LanternActivate()
    {
        isLanternActive = true;
        clickTime = 0f;
    }


    public void RecoverLight(float amountLight)
    {
        currentLight = Mathf.Clamp(currentLight + amountLight, 0, startingLight);

        UIController.instance.UpdateLantern(currentLight, startingLight);
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLantern : MonoBehaviour
{
    [Header("Light Duration (Seconds)")]
    // [SerializeField] private float startingLight; // Value light in seconds
    // [SerializeField] private float startingLight; // Value light in seconds
    [SerializeField] public int currentLight;
    [SerializeField] private int startingLight; // Value light in seconds
    private bool isLanternActive = false;
    private float clickTime = 0f;
    public LayerMask enemy;
    private StarterAssets.StarterAssetsInputs _input;



    // Start is called before the first frame update
    void Start()
    {
        currentLight = startingLight;
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();
    }


    void Update()
    {

        // Enable use of Lantern
        if (_input.click)
        {
            LanternActivate();
            _input.click=false;
        }

        // Lantern in usage
        //LanternState();


        // Disable use of Lantern
        if (!_input.click)
        {
            LanternDeactivate();
        }

        // Update UI 
        currentLight = Mathf.Clamp(currentLight, 0, startingLight);

        //UIController.instance.UpdateLantern(currentLight, startingLight);
    }

    private void LanternDeactivate()
    {
        Debug.Log("Lantern OFF!");
        isLanternActive = false;
    }

    // void LanternState()
    // {
    //     if (isLanternActive)
    //     {
    //         clickTime += Time.deltaTime;
    //         currentLight -= Time.deltaTime;


    //         if (currentLight >= 0)
    //         {
    //             Debug.Log("Temps de clic : " + clickTime);
    //         }
    //         else
    //         {
    //             Debug.Log("Out of Light !");
    //         }
    //     }
    // }

    void LanternActivate()
    {
        isLanternActive = true;
        Debug.Log("<Color=red>Lantern ON!</color>");
        clickTime = 0f;
        

        Collider[] colliding =Physics.OverlapSphere(transform.position,10f,enemy);
        if(colliding.Length>0)
        {
            foreach (var item in colliding)
            {
                if(item.transform.GetComponent<Monstre>()!=null)
                {
                    Debug.Log("Monster found");
                    item.transform.GetComponent<Monstre>().isStunned=true;

                }
            }
        }
        currentLight--;
    }


    public void RecoverLight(int amountLight)
    {
        currentLight = Mathf.Clamp(currentLight + amountLight, 0, startingLight);

        //UIController.instance.UpdateLantern(currentLight, startingLight);
    }

}



using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform pointOfOrigin;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject ObjectToThrow;
    [SerializeField] private GameObject projectileContainer;
    [SerializeField] private SO_Projectile so_projectile;
    [Header("Settings")]
    [SerializeField] private  int projectileCount;
    [SerializeField] private float projectileCoolDown;
    private bool readyToThrow;

    //Potential SO projectile stat for types
    //Potential SO Player for stats
    [Header("Throw")]
    [SerializeField] private  float projectileForce;
    [SerializeField] private  float projectileUpwardForce;

    void Awake()
    {
        if(so_projectile!=null)
        {
            Initilize();
        }
    }
    void Start()
    {
        inputManager=InputManager.Instance;
        readyToThrow = true;
    }
    void Update()
    {
        if(inputManager.buttonShoot>0f && readyToThrow)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow=false;
        Debug.Log("Throwing!");
        GameObject projectile = Instantiate(ObjectToThrow,pointOfOrigin.position,cam.transform.rotation,projectileContainer.transform);
        Rigidbody projectileRb= projectile.GetComponent<Rigidbody>();
        UnityEngine.Vector3 forceDirection=cam.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,500f))
        {

        }
        UnityEngine.Vector3 forceToAdd =forceDirection*projectileForce + transform.up * projectileUpwardForce;
        projectileRb.AddForce(forceToAdd,ForceMode.Impulse);
        Invoke(nameof(ResetThrow),projectileCoolDown);
    }

    private void ResetThrow()
    {
        readyToThrow=true;
    }

    private void Initilize()
    {
        ObjectToThrow=so_projectile.projectile_GameObject;
        projectileCoolDown =so_projectile.cooldown;
    }
}

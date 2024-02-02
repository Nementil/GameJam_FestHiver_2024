using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private SO_Player player;
    [SerializeField] GameEvent health_event;

    [field:Header("Stats")]
    [SerializeField] private int health{set;get;}
    [SerializeField] private int currenthealth {set;get;}

    void Awake()
    {
        health=player.health;
        currenthealth=player.currentHealth;
    }
    public void GetHealth()
    {

    }
    public void SetHealth(int health)
    {
        this.health=health;
        health_event.Raise(this,health.ToString());
    }

    //Dans le UI qui ecoute rajoute
    /*
    public void UpdateText(Component sender,object data)
    {

    }

    */

}

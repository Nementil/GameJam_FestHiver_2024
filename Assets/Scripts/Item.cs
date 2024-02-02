using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Reference")]
    public GameObject item;
    public SO_Item so_item;
    private Collider cl;

    [Header("Stats")]
    public int hp;
    public int ressource;


    void Awake()
    {
        cl=GetComponent<Collider>();
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag=="Player")
        {
            GameObject player=other.gameObject;
            player.GetComponent<Player_Item>().Consume(this);
            Destroy(this.gameObject);
        }
    }
}
public enum ItemType
{
    Health,
    Weapon,
    Ressource,
}

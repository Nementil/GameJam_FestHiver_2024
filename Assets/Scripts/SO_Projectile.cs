using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName ="SO_Projectile", menuName="Scriptable Objects/Projectiles")]
public class SO_Projectile : ScriptableObject
{
    [Header("Reference")]
    public GameObject projectile_GameObject;
    [Header("Stats")]
    public float speed;
    public float damage;
    public int count;
    public int currentCount;
    public float reloadSpeed;
    public float cooldown;
    public float force;
    public bool gravity;
    public bool kinematic;
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName ="SO_Player", menuName="Scriptable Objects/Player")]

public class SO_Player : ScriptableObject
{
    [Header("References")]
    [SerializeField] public SO_Projectile currentProjectile; 
    [SerializeField] public List<SO_Projectile> projectiles;
    [Header("Stats")]
    [SerializeField] public float speed;
    [SerializeField] public float jumpForce;
    [SerializeField] public int health;
    [SerializeField] public int currentHealth;
    [SerializeField] public int ressourceCurrent;
}

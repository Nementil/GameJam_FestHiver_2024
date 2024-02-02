using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable/SO_Item")]
public class SO_Item : ScriptableObject 
{
    public int hp;
    public int ressource;

    public GameObject gm;

    public ItemType type;    

}

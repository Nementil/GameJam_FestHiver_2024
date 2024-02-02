using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Item : MonoBehaviour
{
    public SO_Player so_player;

    public void Consume(Item item)
    {
        if(item.so_item.type==ItemType.Health)
        {
            so_player.health+=item.hp;
        }
    }
}

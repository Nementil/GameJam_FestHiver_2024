using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners =new List<GameEventListener>();
    public void Raise(Component sender,object data)
    {
        for(int i=listeners.Count-1;i>=0;i--)
        {
            if(listeners[i]==null){Debug.Log($"<color=red>WARNING</color> Empty listener slot in SO {this.name}");}
            //Debug.Log($"Listening:{data.GetType()}");
            listeners[i].OnEventRaised(sender, data);
        }
        if(listeners.Count==0)
        {
            Debug.Log($"No listeners on {this.name}");
        }
        //Debug.Log($"Type is {listeners[0].GetType()}");
    }

    public void Raise() 
    {
        Raise(null, null);
    }

    public void Raise(object data) 
    {
        Raise(null, data);
    }

    public void Raise(Component sender) 
    {
        Raise(sender, null);
    }

    public void RegisterListeners(GameEventListener listener)
    {
        if(!listeners.Contains(listener))
        {
            Debug.Log("Adding");
            listeners.Add(listener);
        }
    } 
    public void UnregisterListener(GameEventListener listener)
    {
        if(listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}

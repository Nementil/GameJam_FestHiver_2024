using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class State 
{
    public virtual StateController sc{get;set;}
    public virtual string stateName{get;set;}
    public void OnStateEnter()
    {
        // Code placed here will always run
        
        OnEnter();
    }
    public void OnStateUpdate()
    {
        // Code placed here will always run
        OnUpdate();
    }
    public void OnStateExit()
    {
        // Code placed here will always run
        OnExit();
    }

    public virtual void OnEnter()
    {
        Debug.Log($"Entering {this.stateName}");
    }
    public virtual void OnExit()
    {
        Debug.Log($"Exiting {this.stateName}");
    }
    public virtual void OnUpdate()
    {

    }
}

public class Chase:State
{
    public override StateController sc{get;set;}
    public override string stateName {get;set;}
    public Monstre _monstre;
    public float timer=5f;
    public float currentTimer;
    public Chase(Monstre monstre)
    {
        stateName="Chase";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        _monstre.agent.autoBraking=false;
        _monstre.agent.speed=_monstre.player.GetComponent<FirstPersonController>().MoveSpeed-2;
        Debug.Log("Is Chasing!");
        currentTimer=timer;
    }
    public override void OnExit()
    {
        Debug.Log($"<color=green>Anger +1=> {_monstre.rageCount}</color>");
        _monstre.rageCount++;
    }
    public override void OnUpdate()
    {
        _monstre.agent.destination=_monstre.player.transform.position;
        currentTimer-=Time.deltaTime;
        //Debug.Log($"timer is at{currentTimer}");
        if(currentTimer<=0)
        {
            Debug.Log("Times up");
            currentTimer=timer;
        }
    }
}
public class Stalk:State
{
    public override StateController sc{get;set;}
    public override string stateName {get;set;}
    public Monstre _monstre;
    float[] checkpointDistance;
    public Stalk(Monstre monstre)
    {
        stateName="Stalk";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        Debug.Log("Is Stalking!");
        _monstre.agent.autoBraking=false;
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        checkpointDistance = new float[_monstre.checkpoint.Length];
        if(_monstre.checkpoint.Length==0)
        {
            UnityEngine.Debug.Log("<color=red>No checkpoint in store Error<color/>");
            return;
        }
        if(_monstre.agent.remainingDistance<=_monstre.agent.stoppingDistance||_monstre.agent.destination==null)
        {
            Vector3 point;
            if(_monstre.RandomPoint(_monstre.transform.position,15f,out point))
            {
                _monstre.agent.SetDestination(point);
            }
        }
    }
}
public class Attack:State
{
    public override StateController sc{get;set;}
    public override string stateName {get;set;}
    public Monstre _monstre;
    public Attack(Monstre monstre)
    {
        stateName="Attack";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        //Attack Player, remove hp
        Debug.Log("Is Attacking!");
        
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        Debug.Log("Player in Attack Range!");
    }
}
public class Angry:State
{
    public override StateController sc{get;set;}
    public override string stateName {get;set;}
    public Monstre _monstre;
    public Angry(Monstre monstre)
    {
        stateName="Angry";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        Debug.Log("Is Angry!");
        _monstre.agent.speed=_monstre.player.GetComponent<FirstPersonController>().SprintSpeed-2;
    }
    public override void OnExit()
    {
        _monstre.agent.speed=_monstre.player.GetComponent<FirstPersonController>().MoveSpeed-2;
        _monstre.rageCount=0;
    }
    public override void OnUpdate()
    {
        _monstre.agent.destination=_monstre.player.transform.position;
    }
}
public class Respawn:State
{
    public override StateController sc{get;set;}
    public override string stateName {get;set;}
    public Monstre _monstre;
    public Respawn(Monstre monstre)
    {
        stateName="Respawn";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        Debug.Log("Respawning");
        float[] checkpointDistance=new float[_monstre.checkpoint.Length];
        int index=0;
        // if(Vector3.Distance(_monstre.transform.position,_monstre.player.transform.position)>_monstre.respawnDistance)
        // {
        for(int i=0;i>_monstre.checkpoint.Length-1;i++)
        {
            checkpointDistance[i]=Vector3.Distance(_monstre.transform.position,_monstre.checkpoint[i].transform.position);
        }
        float temp=checkpointDistance[0];
        for(int j=0;j<checkpointDistance.Length-1;j++)
        {
            if(checkpointDistance[j]<temp)
            {
                temp=checkpointDistance[j];
                index=j;
            }
        }
        _monstre.transform.position=_monstre.checkpoint[index].transform.position;
        // }
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        //Debug.Log($"Is Respawning at {_monstre.checkpoint[0].transform.name}!");
    }
}
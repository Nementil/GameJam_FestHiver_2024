using System.Collections;
using System.Collections.Generic;
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
    public Chase(Monstre monstre)
    {
        stateName="Chase";
        _monstre=monstre;
    }
    public override void OnEnter()
    {
        Debug.Log("Is Chasing!");
    }
    public override void OnExit()
    {
        _monstre.rageCount++;
    }
    public override void OnUpdate()
    {
        _monstre.agent.speed=_monstre.speedMonster;
        _monstre.agent.acceleration=_monstre.accelerationMonster;
        _monstre.agent.destination=_monstre.player.transform.position;
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
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        checkpointDistance = new float[_monstre.checkpoint.Length];
        _monstre.agent.autoBraking=false;
        if(_monstre.checkpoint.Length==0)
        {
            UnityEngine.Debug.Log("<color=red>No checkpoint in store Error<color/>");
            return;
        }
        if(Vector3.Distance(_monstre.transform.position,_monstre.player.transform.position)>500f)
        {
            for(int i=0;i>_monstre.checkpoint.Length;i++)
            {
                checkpointDistance[i]=Vector3.Distance(_monstre.transform.position,_monstre.checkpoint[i].transform.position);
            }
            float temp=checkpointDistance[0];
            int index=0;
            for(int j=0;j<checkpointDistance.Length;index++)
            {
                if(checkpointDistance[j]<=temp)
                {
                    temp=checkpointDistance[j];
                    index=j;
                }
            }
            _monstre.transform.position=_monstre.checkpoint[index].transform.position;
        }
        if(_monstre.agent.remainingDistance<=_monstre.agent.stoppingDistance||_monstre.agent.destination==null)
        {
            Vector3 point;
            if(_monstre.RandomPoint(_monstre.transform.position,10f,out point))
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
        
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        Debug.Log("Is Stalking!");
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
        
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        Debug.Log("Is Angry!");
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
        
    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {
        Debug.Log($"Is Respawning at {_monstre.checkpoint[0].transform.name}!");
    }
}
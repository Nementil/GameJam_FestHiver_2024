using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public  State currentState=null;
    //public Chase ChaseState = new Chase();
    //public Stalk StalkState = new Stalk();
    private void Start()
    {
        //ChangeState(currentState);
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }
    public void ChangeState(State newState)
    {
        //Debug.Log($"Current state is {currentState.stateName}");
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEnter();
    }

}

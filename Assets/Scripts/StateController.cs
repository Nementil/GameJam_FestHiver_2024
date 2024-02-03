using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    State currentState;
    
    public Chase ChaseState = new Chase();
    private void Start()
    {
        ChangeState(currentState);
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }
    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter(this);
    }

}

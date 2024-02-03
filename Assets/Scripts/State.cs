using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State 
{
    StateController sc;
    public virtual void OnEnter(StateController stateController)
    {
        sc=stateController;
    }
    public virtual void OnExit()
    {

    }
    public virtual void OnUpdate()
    {

    }

}

public class Chase:State
{
    public override void OnEnter(StateController stateController)
    {

    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {

    }
}
public class Stalk:State
{
    public override void OnEnter(StateController stateController)
    {

    }
    public override void OnExit()
    {

    }
    public override void OnUpdate()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : ScriptableObject
{
    public State targetState;
    public virtual void Initialize(StateMachineController controller)
    {
        
    }

    public virtual bool Check()
    {
        
        return false;
    }
    
}
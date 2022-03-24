using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SMTask : ScriptableObject
{
    protected StateMachineController _controller;

    public virtual void Initialize(StateMachineController controller)
    {
        _controller = controller;
    }
    
    public virtual void RunTask()
    {
        Debug.Log($"Running task : {name}");
    }
}
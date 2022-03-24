using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode : ScriptableObject
{
    [SerializeField] protected BTController controller;

    protected bool success;
    
    public virtual void Init(BTController newController)
    {
        controller = newController;
    }
    
    public virtual bool Execute()
    {
        return success;
    }
    
}
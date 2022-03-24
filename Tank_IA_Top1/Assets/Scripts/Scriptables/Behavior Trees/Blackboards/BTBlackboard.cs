using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BTBlackboard : ScriptableObject
{
    protected Dictionary<string, object> values = new Dictionary<string, object>();
    protected BTController controller;

    public virtual void Init(BTController newController)
    {
        controller = newController;
    }

    public T GetValue<T>(string key)
    {
        return (T)values[key];
    }

    public void SetValue<T>(string key, T newValue)
    {
        if (values.ContainsKey(key))
        {
            values[key] = newValue;
        }
    }

    public bool AddValue<T>(string key, T newValue)
    {
        if (!values.ContainsKey(key))
        {
            values.Add(key, newValue);
            return true;
        }

        return false;
    }
}

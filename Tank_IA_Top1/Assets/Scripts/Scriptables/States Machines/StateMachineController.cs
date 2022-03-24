using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private StateMachine _stateMachine;
    void Start()
    {
        _stateMachine.Begin(this);
    }

    void Update()
    {
        _stateMachine.UpdateStateMachine();
    }

    public void ResetStateMachine()
    {
        _stateMachine.Reset();
    }
    
    
}

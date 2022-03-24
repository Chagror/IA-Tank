using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/State Machine")]
public class StateMachine : ScriptableObject
{
    public State[] _states;
    [SerializeField] private State _activeState;
    private StateMachineController _controller;
    
    public void Begin(StateMachineController controller)
    {
        _controller = controller;
        
        foreach (var state in _states)
        {
            state.Initialize(this, controller);
        }

        _activeState = _states[0];
        _states[0].EnterState();
    }

    public void UpdateStateMachine()
    {
        _activeState.Update();
    }

    public void ChangeState(State newState)
    {
        _activeState.ExitState();
        _activeState = newState;
        _activeState.EnterState();
    }

    public void Reset()
    {
        _activeState = _states[0];
        _states[0].EnterState();
    }
    
    
}

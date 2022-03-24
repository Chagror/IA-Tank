using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "AI/State Machine/State")]
public class State : ScriptableObject
{
    [SerializeField] private SMTask[] _updateTasks;
    [SerializeField] private SMTask[] _enterTasks;
    [SerializeField] private SMTask[] _exitTasks;
    [SerializeField] private Transition[] _transitions;

    private StateMachine _stateOwner;
    private StateMachineController _controller;
    
    
    public void EnterState()
    {
        Debug.Log($"Enter state : {name}");
        ExecuteTasks(_enterTasks);
        
    }

    public void Update()
    {
        ExecuteTasks(_updateTasks);
        CheckTransitions();
    }

    public void ExitState()
    {
        Debug.Log($"Exit state : {name}");
        ExecuteTasks(_exitTasks);
    }

    private void ExecuteTasks(SMTask[] tasks)
    {
        if(tasks.Length <= 0) return;
        foreach (var task in tasks)
        {
            task.RunTask();
        }
    }
    
    private void CheckTransitions()
    {
        foreach (var transition in _transitions)
        {
            if (transition.Check())
            {
                _stateOwner.ChangeState(transition.targetState);
            }
        }
    }

    public void Initialize(StateMachine stateMachine, StateMachineController controller)
    {
        _stateOwner = stateMachine;
        _controller = controller;

        foreach (var transition in _transitions)
        {
            transition.Initialize(controller);
        }

        SMTask[] allTasks = _enterTasks.Concat(_updateTasks).ToArray().Concat(_exitTasks).ToArray();
        foreach (var task in allTasks)
        {
            task.Initialize(controller);
        }
        
    }
    
}

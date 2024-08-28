using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine
{
    public event Action<IState> stateChanged;

    private IState _currentState;

    public void ChangeState(IState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.Enter();
        }

        stateChanged?.Invoke(_currentState);
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Execute();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseStateMachine<T, E> : MonoBehaviour, IStateMachine<T> where T : IState where E : Enum
{
    public T CurrentState { get => this._currentState; set => ChangeState(value); }
    private T _currentState;
    public E CurrentStateEnum { get => _currentStateEnum; set => ChangeState(value); }
    private E _currentStateEnum;

    private void Update()
    {
        this.CurrentState?.OnUpdate(Time.deltaTime);
    }

    public virtual void ChangeState(T state)
    {
        if (state == null) { Debug.LogError("Null state passed in " + this.GetType().Name); return; }

        if (this._currentState != null)
        {
            this._currentState.OnExit();
        }

        this._currentState = state;
        this._currentState.OnEnter();
    }
    public virtual void ChangeState(E stateEnum)
    {
        _currentStateEnum = stateEnum;
        this.ChangeState(GetState(stateEnum));
    }

    protected abstract T GetState(E stateEnum);
}

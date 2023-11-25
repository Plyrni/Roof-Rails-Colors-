using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<StateType> where StateType : IState
{
    StateType CurrentState { get; set; }

    public void SetState(StateType state);
}

using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

[System.Serializable]
public class GameState : IState
{
    public virtual void OnEnter()
    {
        LeanTouch.OnGesture += ManageInputs;
    }

    public virtual void OnUpdate(float deltaTime)
    {
    }

    public virtual void OnLateUpdate(float deltaTime)
    {
    }

    public virtual void OnExit()
    {
        LeanTouch.OnGesture -= ManageInputs;
        //Debug.Log("EXIT " + GetType().Name);
    }

    protected virtual void ManageInputs(List<LeanFinger> fingers)
    {
    }
}
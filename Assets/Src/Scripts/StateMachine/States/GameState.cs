using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState : IState
{
    public virtual void OnEnter()
    {
        
    }
    public virtual void OnUpdate(float deltaTime)
    {

    }

    public virtual void OnLateUpdate(float deltaTime)
    {

    }

    public virtual void OnExit()
    {
        Debug.Log("EXIT " + GetType().Name);
    }
}

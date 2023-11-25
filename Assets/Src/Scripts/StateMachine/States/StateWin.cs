using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWin : GameState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Game.DataManager.IncrementLevel();
    }
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

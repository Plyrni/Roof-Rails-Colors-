using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class StateHome : GameState
{
    
    public override void OnEnter()
    {
        base.OnEnter();
        Game.Player.TeamColorManager.SetTeamColor(TeamColor.Yellow);
        Game.MapManager.SpawnLevel(Game.DataManager.GetLevel());
        Game.Player.Reset();
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

    protected override void ManageInputs(List<LeanFinger> fingers)
    {
        base.ManageInputs(fingers);
        LeanFinger finger = fingers[0];
        if (finger.IsActive && !finger.StartedOverGui && finger.Down)
        {
            Game.StateMachine.ChangeState(GameStateEnum.Playing);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
        CinemachineVirtualCamera vcamPlay =Game.CameraManager.Vcam_Play;
        Vector3 newpos = Game.Player.transform.position - Vector3.back * 1f + Vector3.up * 1f;
        
        CinemachineTransposer transposer = vcamPlay.GetCinemachineComponent<CinemachineTransposer>();
        transposer.ForceCameraPosition(newpos,vcamPlay.transform.rotation);
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

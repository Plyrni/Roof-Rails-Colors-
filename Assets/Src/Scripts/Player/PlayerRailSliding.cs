using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRailSliding : MonoBehaviour
{
    [HideInInspector] public UnityEvent onLandOnRail;
    private Player _player;
    private int nbRailEntered = 0;

    public void Reset()
    {
        nbRailEntered = 0;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        Rail.OnMasterRodEnter.AddListener(OnMasterRodEnterRail);
        Rail.OnMasterRodExit.AddListener(OnMasterRodExitRail);
    }

    private void LateUpdate()
    {
        if (Game.State == GameStateEnum.Lose)
        {
            return;
        }
        
        if (nbRailEntered > 0 && _player.MovementComponent.IsGrounded == false)
        {
            if (nbRailEntered == 1)
            {
                if (_player.Region == GameRegion.FinalZone)
                {
                    _player.Kill();
                }
                else
                {
                    Game.StateMachine.ChangeState(GameStateEnum.Lose);
                }
            }
        }
    }

    private void OnMasterRodEnterRail()
    {
        if (Game.State == GameStateEnum.Playing)
        {
            nbRailEntered += 1;
            onLandOnRail?.Invoke();
        }
    }

    private void OnMasterRodExitRail()
    {
        if (Game.State == GameStateEnum.Playing)
        {
            nbRailEntered -= 1;
        }
    }

    private void OnDestroy()
    {
        Rail.OnMasterRodEnter.RemoveListener(OnMasterRodEnterRail);
        Rail.OnMasterRodExit.RemoveListener(OnMasterRodExitRail);
    }
}
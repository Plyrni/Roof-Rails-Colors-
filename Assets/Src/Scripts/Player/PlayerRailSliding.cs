using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRailSliding : MonoBehaviour
{
    [HideInInspector] public UnityEvent onLandOnRail;
    [HideInInspector] public UnityEvent onExitRail;
    public bool IsSliding => _nbRailEntered >= 2;
    private Player _player;
    private int _nbRailEntered = 0;

    public void Reset()
    {
        _nbRailEntered = 0;
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
        
        if (_nbRailEntered > 0 && _player.MovementComponent.IsGrounded == false)
        {
            if (_nbRailEntered == 1)
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
            _nbRailEntered += 1;
            onLandOnRail?.Invoke();
        }
    }

    private void OnMasterRodExitRail()
    {
        if (Game.State == GameStateEnum.Playing)
        {
            _nbRailEntered -= 1;
            onExitRail?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Rail.OnMasterRodEnter.RemoveListener(OnMasterRodEnterRail);
        Rail.OnMasterRodExit.RemoveListener(OnMasterRodExitRail);
    }
}
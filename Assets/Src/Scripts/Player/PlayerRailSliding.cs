using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailSliding : MonoBehaviour
{
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
        // TODO : Check game state
        if (nbRailEntered > 0 && _player.MovementComponent.IsGrounded == false)
        {
            if (nbRailEntered == 1)
            {
                Game.ChangeState(GameState.Lose);
            }
        }
    }

    private void OnMasterRodEnterRail()
    {
        if (Game.State == GameState.Playing)
        {
            nbRailEntered += 1;
        }
    }

    private void OnMasterRodExitRail()
    {
        if (Game.State == GameState.Playing)
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
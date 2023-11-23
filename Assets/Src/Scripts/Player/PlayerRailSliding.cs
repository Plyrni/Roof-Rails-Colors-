using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailSliding : MonoBehaviour
{
    private Player _player;
    private int nbRailEntered = 0;
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
                _player.Rigidbody.constraints = RigidbodyConstraints.None;
                // TODO : Trigger Lose
            }
        }
    }

    private void OnMasterRodEnterRail()
    {
        nbRailEntered += 1;
    }
    private void OnMasterRodExitRail()
    {
        nbRailEntered -= 1;
    }
    private void OnDestroy()
    {
        Rail.OnMasterRodEnter.RemoveListener(OnMasterRodEnterRail);
        Rail.OnMasterRodExit.RemoveListener(OnMasterRodExitRail);
    }
}

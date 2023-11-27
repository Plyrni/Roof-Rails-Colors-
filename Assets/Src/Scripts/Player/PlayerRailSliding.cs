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
    private List<Rail> _railsColliding = new List<Rail>();

    public void Reset()
    {
        _nbRailEntered = 0;
        _railsColliding.Clear();
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        Rail.OnBladeEnter.AddListener(OnBladeEnterRail);
        Rail.OnBladeExit.AddListener(OnBladeExitRail);
        _railsColliding.Capacity = 5;
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
                FailSlide();
            }
            else
            {
                bool foundRailOnRight = false;
                bool foundRailOnLeft = false;

                foreach (var rail in _railsColliding)
                {
                    Direction dirFromBlade = _player.Blade.ComputePointDirection(rail.transform.position);
                    if (dirFromBlade == Direction.Left)
                    {
                        foundRailOnLeft = true;
                    }
                    else
                    {
                        foundRailOnRight = true;
                    }
                }

                if (foundRailOnRight == false || foundRailOnLeft == false)
                {
                    FailSlide();
                }
            }
        }
    }

    private void FailSlide()
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

    private void OnBladeEnterRail(Rail rail)
    {
        if (Game.State == GameStateEnum.Playing)
        {
            _nbRailEntered += 1;
            _railsColliding.Add(rail);
            onLandOnRail?.Invoke();
        }
    }

    private void OnBladeExitRail(Rail rail)
    {
        if (Game.State == GameStateEnum.Playing)
        {
            _nbRailEntered -= 1;
            _railsColliding.Remove(rail);
            onExitRail?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Rail.OnBladeEnter.RemoveListener(OnBladeEnterRail);
        Rail.OnBladeExit.RemoveListener(OnBladeExitRail);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRailSliding : MonoBehaviour
{
    [HideInInspector] public UnityEvent onLandOnRail;
    [HideInInspector] public UnityEvent onExitRail;
    public bool IsSliding => _nbRailEntered >= 2;

    [SerializeField] private ParticleSystem _vfxSparks;
    private Player _player;
    private int _nbRailEntered = 0;
    private readonly List<Rail> _railsColliding = new List<Rail>();
    [SerializeField] private List<RailFX> _listVFX = new List<RailFX>();

    [System.Serializable]
    private class RailFX
    {
        public Rail rail;
        public ParticleSystem vfxSpark;
    }

    public void Reset()
    {
        _nbRailEntered = 0;
        _railsColliding.Clear();
        DestroyAllSparks();
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.onKill.AddListener(OnKill);
        Rail.OnBladeEnter.AddListener(OnBladeEnterRail);
        Rail.OnBladeExit.AddListener(OnBladeExitRail);
        _railsColliding.SetLength(5);
    }

    private void OnKill()
    {
        DestroyAllSparks();
    }

    private void Update()
    {
        if (_nbRailEntered > 0)
        {
            foreach (var railVFX in _listVFX)
            {
                Transform vfxTransform = railVFX.vfxSpark.transform;
                Vector3 newSparkPos = vfxTransform.position;
                newSparkPos.x = railVFX.rail.transform.position.x;
                vfxTransform.position = newSparkPos;
            }
        }
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
            else if (CheckAreRailsOnBothSides() == false)
            {
                FailSlide();
            }
        }
    }

    private bool CheckAreRailsOnBothSides()
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

            if (foundRailOnRight && foundRailOnLeft)
            {
                return true;
            }
        }

        return false;
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
        if (Game.State != GameStateEnum.Playing)
        {
            return;
        }

        _nbRailEntered += 1;
        _railsColliding.Add(rail);

        RailFX railFX = new RailFX();
        railFX.rail = rail;
        railFX.vfxSpark = CreateVFXSpark();
        _listVFX.Add(railFX);

        onLandOnRail?.Invoke();
    }

    private void OnBladeExitRail(Rail rail)
    {
        if (Game.State != GameStateEnum.Playing)
        {
            return;
        }

        _nbRailEntered -= 1;
        _railsColliding.Remove(rail);

        RailFX railFX = _listVFX.FirstOrDefault(rfx => rfx.rail == rail);
        Destroy(railFX.vfxSpark.gameObject);
        _listVFX.Remove(railFX);

        onExitRail?.Invoke();
    }


    private void DestroyAllSparks()
    {
        foreach (var vfx in _listVFX)
        {
            Destroy(vfx.vfxSpark.gameObject);
        }

        _listVFX.Clear();
    }

    private ParticleSystem CreateVFXSpark()
    {
        ParticleSystem newVFX = Instantiate(_vfxSparks, _player.Blade.transform);
        return newVFX;
    }

    private void OnDestroy()
    {
        Rail.OnBladeEnter.RemoveListener(OnBladeEnterRail);
        Rail.OnBladeExit.RemoveListener(OnBladeExitRail);
    }
}
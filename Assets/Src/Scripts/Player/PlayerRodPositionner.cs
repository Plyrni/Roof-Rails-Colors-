using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerRodPositionner : MonoBehaviour
{
    private Player _playerOwner;
    private RodCutable _rod;

    private void Awake()
    {
        _playerOwner = GetComponentInParent<Player>();
        _rod = GetComponent<RodCutable>();
    }

    private void Start()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.AddListener(OnPlayerGroundStateChange);
    }

    private void OnPlayerGroundStateChange(bool newValue)
    {
        if (newValue == true)
        {
            OnPlayerLand();
        }
        else
        {
            OnPlayerFall();
        }
    }
    
    private void OnPlayerFall()
    {
        
    }
    private void OnPlayerLand()
    {
        
    }

    private void OnDestroy()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.RemoveListener(OnPlayerGroundStateChange);
    }
}

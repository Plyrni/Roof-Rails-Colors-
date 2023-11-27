using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Player _player;
    
    private static readonly int _paramIsMoving = Animator.StringToHash("isMoving");
    private static readonly int _paramIsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int _paramLandedOnRail = Animator.StringToHash("landedOnRail");

    private void Awake()
    {
        if (!_player)
            _player = GetComponent<Player>();
        if (!_animator)
            _animator = GetComponentInChildren<Animator>();
        
        _player.MovementComponent.onGroundedStateChange.AddListener(OnGroundedStateChange);
        _player.MovementComponent.onIsMovingChange.AddListener(OnIsMovingChange);
        _player.RailSliding.onLandOnRail.AddListener(OnLandOnRail);
    }

    private void OnLandOnRail()
    {
        _animator.SetTrigger(_paramLandedOnRail);
    }

    private void OnIsMovingChange(bool isMoving)
    {
        _animator.SetBool(_paramIsMoving,isMoving);
    }

    private void OnGroundedStateChange(bool isGrounded)
    {
        _animator.SetBool(_paramIsGrounded,isGrounded);
    }
}

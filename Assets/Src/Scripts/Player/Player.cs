using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    public PlayerMovement MovementComponent => _movement ??= GetComponent<PlayerMovement>();
    public RodCutable Rod => _rod;
    public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
    public PlayerRailSliding RailSliding => _railSliding ??= GetComponent<PlayerRailSliding>();

    [SerializeField] private float _baseRodeSize;
    [SerializeField] private RodCutable _rod;
    private PlayerMovement _movement;
    private Rigidbody _rigidbody;
    private PlayerRailSliding _railSliding;

    private void Awake()
    {
        Game.OnChangeState.AddListener(OnChangeState);
        Reset();
    }

    public void Reset()
    {
        Rigidbody.MovePosition(Vector3.zero + Vector3.up * 0.01f);
        Rigidbody.MoveRotation(Quaternion.identity);
        MovementComponent.Reset();
        RailSliding.Reset();
        _rod.SetSize(_baseRodeSize);
    }

    private void OnChangeState(GameState newState)
    {
        if (newState == GameState.Lose)
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnDestroy()
    {
        Game.OnChangeState.RemoveListener(OnChangeState);
    }
}
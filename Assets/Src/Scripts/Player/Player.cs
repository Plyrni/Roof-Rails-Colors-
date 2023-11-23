using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    public PlayerMovement MovementComponent => _movement;
    public RodCutable Rod => _rod;
    public Rigidbody Rigidbody => _rigidbody;

    [SerializeField] private float sensivity;
    [SerializeField] private float _baseRodeSize;
    [SerializeField] private RodCutable _rod;
    private PlayerMovement _movement;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
        Game.OnChangeState.AddListener(OnChangeState);
        Reset();
    }

    public void Reset()
    {
        this.transform.position = Vector3.zero + Vector3.up * 0.01f;
        _rod.SetSize(2f);
    }

    private void OnChangeState(GameState newState)
    {
        if (newState == GameState.Lose)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnDestroy()
    {
        Game.OnChangeState.RemoveListener(OnChangeState);
    }
}
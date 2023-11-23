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
    [SerializeField] private RodCutable _rod;
    private PlayerMovement _movement;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
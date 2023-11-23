using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    [SerializeField] private float sensivity;
    [SerializeField] private RodCutable _rod;

    private void Awake()
    {
    }

    void Start()
    {
    }
}
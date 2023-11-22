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

    private float timeToCut = 2f;
    private float currentTime;
    private bool _hasCut;

    void Update()
    {
        if (!_hasCut)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeToCut)
            {
                _rod.CutFromHitPos(_rod.transform.position + _rod.transform.up * -0.5f);
                _hasCut = true;
            }
        }
    }
}
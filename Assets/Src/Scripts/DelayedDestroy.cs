using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [SerializeField] public float timeToDestroy;

    protected virtual void Start()
    {
        Destroy(gameObject,timeToDestroy);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rail : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnMasterRodEnter = new UnityEvent();
    [HideInInspector] public static UnityEvent OnMasterRodExit = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rod"))
        {
            OnMasterRodEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rod"))
        {
            OnMasterRodExit?.Invoke();
        }
    }
}
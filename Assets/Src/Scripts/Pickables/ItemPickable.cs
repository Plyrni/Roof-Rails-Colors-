using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEditor.UIElements;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    [TagField]
    [SerializeField] private List<string> tagsAccepted;


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (tagsAccepted.Any(str => other.CompareTag(str)))
        {
            OnValidColliderDetected(other);
        }
    }

    protected virtual void OnValidColliderDetected(Collider other)
    {
        Debug.Log("Detect " + other);
    }
}
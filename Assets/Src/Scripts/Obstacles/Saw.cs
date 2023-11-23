using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Saw : MonoBehaviour
{
    private static readonly int ParamIsRotating = Animator.StringToHash("IsRotating");
    private const float HalfSize = 0.225f;
    
    [SerializeField] private float _ejectionForce = 35f;
    [SerializeField] private float _ejectionTorqueForce = 100f;
    [FormerlySerializedAs("animator")] [SerializeField] private Animator _animator;
    
    private bool _triggerEntered = false;

    private void Awake()
    {
        EnableRotation(true);
    }

    private void EnableRotation(bool enable)
    {
        _animator.SetBool(ParamIsRotating, enable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rod"))
        {
            RodCutable rodCutable = other.gameObject.GetComponent<RodCutable>();
            if (rodCutable != null)
            {
                _triggerEntered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rod"))
        {
            RodCutable rodCutable = other.gameObject.GetComponent<RodCutable>();
            if (rodCutable != null)
            {
                _triggerEntered = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_triggerEntered == false)
        {
            return;
        }

        if (other.gameObject.CompareTag("Rod"))
        {
            RodCutable rodCutable = other.gameObject.GetComponent<RodCutable>();
            CutRodThatEntered(rodCutable);
        }
    }

    private void CutRodThatEntered(RodCutable rod)
    {
        if (rod != null)
        {
            Vector3 playerDirection = Game.Instance.player.transform.position - this.transform.position;
            Vector3 offset = (Vector3.right * Mathf.Sign(playerDirection.x)) * HalfSize;
            Vector3 cutPos = this.transform.position + offset;
            GameObject fakeRod = rod.CutFromHitPos(cutPos);

            EjectCutRod(fakeRod, cutPos);
        }
    }

    private void EjectCutRod(GameObject cutRod, Vector3 cutPos)
    {
        Rigidbody rigid = cutRod.GetComponent<Rigidbody>();

        if (rigid == null)
        {
            return;
        }

        Vector3 dirEjection = (-this.transform.forward + -Vector3.up * 0.25f).normalized * _ejectionForce;
        rigid.AddForce(dirEjection, ForceMode.Impulse);
        
        Vector3 torqueDir = -(cutRod.transform.position - cutPos).normalized.x * Vector3.up;
        rigid.AddTorque(torqueDir * _ejectionTorqueForce,
            ForceMode.Impulse);
    }
}
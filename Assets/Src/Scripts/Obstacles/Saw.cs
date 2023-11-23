using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private static readonly int ParamIsRotating = Animator.StringToHash("IsRotating");
    private const float halfSize = 0.225f;

    [SerializeField] private Animator animator;
    private bool triggerEntered = false;

    private void Awake()
    {
        EnableRotation(true);
    }

    private void EnableRotation(bool enable)
    {
        animator.SetBool(ParamIsRotating, enable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rod"))
        {
            RodCutable rodCutable = other.gameObject.GetComponent<RodCutable>();
            if (rodCutable != null)
            {
                triggerEntered = true;
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
                triggerEntered = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerEntered == false)
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
            Vector3 offset = (Vector3.right * Mathf.Sign(playerDirection.x)) * halfSize;
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

        Vector3 dirEjection = (-this.transform.forward + Vector3.up * 0.75f).normalized * 35f;
        rigid.AddForce(dirEjection, ForceMode.Impulse);
        rigid.AddTorque(-(cutRod.transform.position - cutPos).normalized.x * Vector3.up * 100f, ForceMode.Impulse);
    }
}
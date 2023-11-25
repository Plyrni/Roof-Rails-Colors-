using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rail : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnMasterRodEnter = new UnityEvent();
    [HideInInspector] public static UnityEvent OnMasterRodExit = new UnityEvent();
    
    [SerializeField] private float  _bumpForce;
    private bool _isPenetratedByRod;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rod"))
        {
            _isPenetratedByRod = true;
            OnMasterRodEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rod"))
        {
            _isPenetratedByRod = false;
            OnMasterRodExit?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Player player = Game.Player;
            if (IsPlayerBumpable(player))
            {
                // Compute bump direction
                Vector3 localPosPlayer = player.transform.position - this.transform.position;
                float dot = Vector3.Dot(localPosPlayer.x * Vector3.right, this.transform.forward);
                Vector3 bumpDirection = dot > 0 ? this.transform.forward : -this.transform.forward;
                
                player.MovementComponent.Bump(bumpDirection + Vector3.down * 0.25f,_bumpForce);
                player.MovementComponent.DisableInputs(0.2f);
            }
        }
    }

    private bool IsPlayerBumpable(Player player)
    {
        /// Compute is player on top
        Vector3 localPosPlayer = player.transform.position - this.transform.position;
        Vector3 playerHeightProjection = Vector3.Project(localPosPlayer, transform.up);
        if (localPosPlayer.y > playerHeightProjection.y)
        {
            return true;
        }
        return false;
    }
}
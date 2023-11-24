using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float bumpForce = 10;
    
    private int nbValidColliderThatCollide = 0;
    private bool _isCollidingWithValidCollider = false;
    
    private void BumpPlayer(Player player, Collision collision)
    {
        Vector3 bumpDir = collision.GetContact(0).normal;
        player.MovementComponent.Bump(bumpDir, 10);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (IsValidCollider(other.collider))
        {
            _isCollidingWithValidCollider = true;
            nbValidColliderThatCollide++;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (IsValidCollider(other.collider))
        {
            nbValidColliderThatCollide--;
            if (nbValidColliderThatCollide <= 0)
            {
                _isCollidingWithValidCollider = false;
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_isCollidingWithValidCollider == false)
        {
            return;
        }

        if (IsValidCollider(other.collider))
        {
            OnValidCollisionStay(other);
        }
    }

    private void OnValidCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            BumpPlayer(Game.Player,other);
        }
    }

    
    protected virtual bool IsValidCollider(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }
}

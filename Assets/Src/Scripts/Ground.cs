using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            float posX = transform.position.x;
            float halfSize = transform.lossyScale.x / 2;
            Game.Player.MovementComponent.SetMovementBounds(posX - halfSize, posX + halfSize);
        }
    }
}
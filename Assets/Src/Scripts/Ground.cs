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
        else if (other.collider.CompareTag("Rod"))
        {
            Game.StateMachine.ChangeState(GameStateEnum.Lose);
            Vector3 bumpDir = Game.Player.transform.position.x > this.transform.position.x
                ? transform.right
                : -transform.right;
            Game.Player.Rigidbody.AddForce((bumpDir + Vector3.forward).normalized * 3f,ForceMode.Impulse);
        }
    }
}
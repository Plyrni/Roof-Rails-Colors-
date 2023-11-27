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
            Player player = Game.Player;
            if (Game.State == GameStateEnum.Playing && player.Region != GameRegion.FinalZone)
            {
                Game.StateMachine.ChangeState(GameStateEnum.Lose);
                Vector3 bumpDir = player.transform.position.x > this.transform.position.x
                    ? transform.right
                    : -transform.right;
                player.Rigidbody.AddForce((bumpDir + Vector3.forward).normalized * 3f, ForceMode.Impulse);
            }
        }
    }
}
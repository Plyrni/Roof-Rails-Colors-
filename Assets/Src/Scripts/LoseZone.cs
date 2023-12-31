using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Game.State != GameStateEnum.Lose && Game.State != GameStateEnum.Win)
        {
            Game.StateMachine.ChangeState(GameStateEnum.Lose);
        }
    }
}
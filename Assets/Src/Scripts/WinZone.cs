using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private float winMultiplier = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Game.State != GameStateEnum.Win)
        {
            Game.DataManager.SetWinMultiplier(winMultiplier);
            Game.StateMachine.ChangeState(GameStateEnum.Win);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    private void Awake()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnChangeState);        
    }

    private void OnChangeState(GameStateEnum newStateEnum)
    {
        if (newStateEnum == GameStateEnum.Win)
        {
            // Apply mult
        }
    }
}

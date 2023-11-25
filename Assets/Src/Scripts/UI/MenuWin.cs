using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuWin : MonoBehaviour
{
    [SerializeField] private Button btnNext;

    private void Awake()
    {
        btnNext.onClick.AddListener(OnClickBtnNext);
    }

    private void OnClickBtnNext()
    {
        Game.StateMachine.ChangeState(GameStateEnum.Home);
    }

    private void OnDestroy()
    {
        btnNext.onClick.RemoveListener(OnClickBtnNext);
    }
}

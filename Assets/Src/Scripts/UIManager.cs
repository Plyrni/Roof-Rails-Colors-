using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class UIManager : MonoBehaviour
{
    [SerializedDictionary("State", "Menu")]
    [SerializeField]private SerializedDictionary<GameStateEnum, GameObject> menuByState;

    private GameObject _currentMenu;

    private void Start()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnChangeState);
    }

    private void OnChangeState(GameStateEnum newStateEnum)
    {
        OpenStateMenu(newStateEnum);
    }

    private void OpenStateMenu(GameStateEnum stateEnum)
    {
        GameObject menuToOpen = null;
        if (menuByState.TryGetValue(stateEnum, out menuToOpen))
        {
            HideAllMenuExcept(stateEnum);
            menuToOpen.SetActive(true);
            _currentMenu = menuToOpen;
        }     
    }

    private void HideAllMenuExcept(GameStateEnum stateEnumException)
    {
        foreach (var pair in menuByState)
        {
            if (pair.Key != stateEnumException)
            {
                pair.Value.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        Game.StateMachine.OnStateChanged.RemoveListener(OnChangeState);
    }
}
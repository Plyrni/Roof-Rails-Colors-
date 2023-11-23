using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class UIManager : MonoBehaviour
{
    [SerializedDictionary("State", "Menu")]
    [SerializeField]private SerializedDictionary<GameState, GameObject> menuByState;

    private GameObject _currentMenu;

    private void Awake()
    {
        Game.OnChangeState.AddListener(OnChangeState);
    }

    private void OnChangeState(GameState newState)
    {
        OpenStateMenu(newState);
    }

    private void OpenStateMenu(GameState state)
    {
        GameObject menuToOpen = null;
        if (menuByState.TryGetValue(state, out menuToOpen))
        {
            HideAllMenuExcept(state);
            menuToOpen.SetActive(true);
            _currentMenu = menuToOpen;
        }     
    }

    private void HideAllMenuExcept(GameState stateException)
    {
        foreach (var pair in menuByState)
        {
            if (pair.Key != stateException)
            {
                pair.Value.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        Game.OnChangeState.RemoveListener(OnChangeState);
    }
}
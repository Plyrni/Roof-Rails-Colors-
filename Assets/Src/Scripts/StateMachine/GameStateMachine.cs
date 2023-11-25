using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class GameStateMachine : ABaseStateMachine<GameState, GameStateEnum>
{
    [SerializeReference] private GameState stateHome;
    [SerializeReference] private GameState statePlaying;
    [SerializeReference] private GameState stateWin;
    [SerializeReference] private GameState stateLose;

    [HideInInspector] public UnityEvent<GameStateEnum> OnStateChanged;

    private void LateUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.OnLateUpdate(Time.deltaTime);
        }
    }

    protected override GameState GetState(GameStateEnum stateEnum)
    {
        GameState tempState = null;

        switch (stateEnum)
        {
            case GameStateEnum.Home:
                tempState = stateHome;
                break;
            case GameStateEnum.Playing:
                tempState = statePlaying;
                break;
            case GameStateEnum.Win:
                tempState = stateWin;
                break;
            case GameStateEnum.Lose:
                tempState = stateLose;
                break;
        }

        if (tempState == null)
        {
            Debug.LogError("[GameStateMachine] State not found : " + stateEnum);
        }

        return tempState;
    }

    public override void ChangeState(GameState state)
    {
        base.ChangeState(state);
        Debug.Log("[GameStateMachine] New GameState : " + CurrentStateEnum);
        this.OnStateChanged?.Invoke(this.CurrentStateEnum);
    }

    private void OnDestroy()
    {
        OnStateChanged.RemoveAllListeners();
    }
}
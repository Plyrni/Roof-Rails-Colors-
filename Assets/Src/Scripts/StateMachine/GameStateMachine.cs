using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class GameStateMachine : ABaseStateMachine<GameState, GameStateEnum>
{
    [SerializeReference] private GameState stateHome;
    [SerializeReference] private GameState statePlaying;
    [SerializeReference] private GameState stateWin;
    [SerializeReference] private GameState stateLose;

    [HideInInspector] public UnityEvent<GameStateEnum> OnGameStateChanged;

    private void Awake()
    {
        this.SetState(GameStateEnum.Home);
    }

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
            Debug.LogError("[GameStateMachine] State not found");
        }

        return tempState;
    }

    public override void SetState(GameState state)
    {
        base.SetState(state);
        this.OnGameStateChanged?.Invoke(this.CurrentStateEnum);
    }
}
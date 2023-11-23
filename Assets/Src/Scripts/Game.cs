using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class Game : MonoBehaviour
{
    public static Game Instance => instance;
    public static Player Player => Instance._player;
    public static GameState State => Instance._state;
    public static DataManager DataManager => Instance._dataManager;
    public static readonly UnityEvent<GameState> OnChangeState = new UnityEvent<GameState>();

    private static Game instance;
    [SerializeField] private Player _player;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private DataManager _dataManager;
    private GameState _state;

    private void Awake()
    {
        instance = this;
        ChangeState(GameState.Home);
        LeanTouch.OnGesture += ManageInputs;
    }

    public static void ChangeState(GameState newState)
    {
        if (newState == State)
        {
            return;
        }

        instance._state = newState;
        _OnChangeState(State);
        OnChangeState?.Invoke(newState);
    }

    /// <summary>
    /// Do whatever needs to be done, but do it before everything.
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="ArgumentOutOfRangeException">Switch needs maintenance</exception>
    private static void _OnChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.NONE:
                Debug.LogError("You better no be there");
                break;
            case GameState.Home:
                instance._mapManager.SpawnLevel(DataManager.GetLevel());
                break;
            case GameState.Playing:
                break;
            case GameState.Win:
                DataManager.IncrementLevel();
                break;
            case GameState.Lose:
                break;
        }
    }

    private void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        switch (State)
        {
            case GameState.Home:
                if (finger.IsActive && !finger.StartedOverGui)
                {
                    ChangeState(GameState.Playing);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        LeanTouch.OnGesture -= ManageInputs;
    }
}
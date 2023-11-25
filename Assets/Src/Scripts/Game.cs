using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class Game : MonoBehaviour
{
    public static Game Instance => instance;
    public static Player Player => Instance._player;
    public static GameState State => Instance._state;
    public static DataManager DataManager => Instance._dataManager;
    public static Scene Map => instance._mapManager.CurrentScene;
    public static Transform MapTransform => Map.GetRootGameObjects()[0].transform;
    public static readonly UnityEvent<GameState> OnChangeState = new UnityEvent<GameState>();

    private static Game instance;
    [SerializeField] private Player _player;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private DataManager _dataManager;
    private TeamColorManager _teamColorManager;
    private GameState _state;

    private void Awake()
    {
        instance = this;
        ChangeState(GameState.Home);
        LeanTouch.OnGesture += ManageInputs;

        Application.targetFrameRate =-1;
    }

    public static void ChangeState(GameState newState)
    {
        if (newState == State)
        {
            return;
        }

        instance._state = newState;
        _OnChangeState(State);
        Debug.Log("[Game] New GameState : " + State);
        OnChangeState?.Invoke(newState);
    }

    /// <summary>
    /// Do whatever needs to be done, but do it before the event is called.
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="ArgumentOutOfRangeException">Switch needs maintenance</exception>
    private static void _OnChangeState(GameState newState)
    {
        // TODO : StateMachine
        switch (newState)
        {
            case GameState.NONE:
                Debug.LogError("How are you even here ?");
                break;
            case GameState.Home:
                Player.TeamColorManager.SetTeamColor(TeamColor.Yellow);
                instance._mapManager.SpawnLevel(DataManager.GetLevel());
                Player.Reset();
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

    protected virtual void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        
        // TODO : StateMachine
        switch (State)
        {
            case GameState.Home:
                if (finger.IsActive && !finger.StartedOverGui && finger.Down)
                {
                    ChangeState(GameState.Playing);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        LeanTouch.OnGesture -= ManageInputs;
        OnChangeState.RemoveAllListeners();
    }
}
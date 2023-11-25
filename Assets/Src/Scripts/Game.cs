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
    public static GameStateEnum StateEnum => Instance.stateEnum;
    public static DataManager DataManager => Instance._dataManager;
    public static Scene Map => instance._mapManager.CurrentScene;
    public static Transform MapTransform => Map.GetRootGameObjects()[0].transform;
    public static readonly UnityEvent<GameStateEnum> OnChangeState = new UnityEvent<GameStateEnum>();

    private static Game instance;
    [SerializeField] private Player _player;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private DataManager _dataManager;
    private TeamColorManager _teamColorManager;
    private WinManager _winManager; // Should later be replaced by a stateMachine
    private GameStateEnum stateEnum;
    
    private void Awake()
    {
        instance = this;
        ChangeState(GameStateEnum.Home);
        LeanTouch.OnGesture += ManageInputs;

        Application.targetFrameRate =-1;
    }

    public static void ChangeState(GameStateEnum newStateEnum)
    {
        if (newStateEnum == StateEnum)
        {
            return;
        }

        instance.stateEnum = newStateEnum;
        _OnChangeState(StateEnum);
        Debug.Log("[Game] New GameState : " + StateEnum);
        OnChangeState?.Invoke(newStateEnum);
    }

    /// <summary>
    /// Do whatever needs to be done, but do it before the event is called.
    /// </summary>
    /// <param name="newStateEnum"></param>
    /// <exception cref="ArgumentOutOfRangeException">Switch needs maintenance</exception>
    private static void _OnChangeState(GameStateEnum newStateEnum)
    {
        // TODO : StateMachine
        switch (newStateEnum)
        {
            case GameStateEnum.NONE:
                Debug.LogError("How are you even here ?");
                break;
            case GameStateEnum.Home:
                Player.TeamColorManager.SetTeamColor(TeamColor.Yellow);
                instance._mapManager.SpawnLevel(DataManager.GetLevel());
                Player.Reset();
                break;
            case GameStateEnum.Playing:
                break;
            case GameStateEnum.Win:
                DataManager.IncrementLevel();
                break;
            case GameStateEnum.Lose:
                break;
        }
    }

    protected virtual void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        
        // TODO : StateMachine
        switch (StateEnum)
        {
            case GameStateEnum.Home:
                if (finger.IsActive && !finger.StartedOverGui && finger.Down)
                {
                    ChangeState(GameStateEnum.Playing);
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
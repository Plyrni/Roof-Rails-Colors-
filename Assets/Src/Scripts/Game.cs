using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class Game : MonoBehaviour
{
    public static Game Instance => instance;
    public static Player Player => Instance._player;
    public static GameStateEnum State => Instance._stateMachine.CurrentStateEnum;
    public static GameStateMachine StateMachine => Instance._stateMachine;
    public static DataManager DataManager => Instance._dataManager;
    public static Scene Map => instance._mapManager.CurrentScene;
    public static Transform MapTransform => Map.GetRootGameObjects()[0].transform;
    public static MapManager MapManager => instance._mapManager;

    private static Game instance;

    [SerializeField] private Player _player;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private DataManager _dataManager;
    [SerializeField] private GameStateMachine _stateMachine;
    private TeamColorManager _teamColorManager;

    private void Awake()
    {
        instance = this;
        _stateMachine = GetComponent<GameStateMachine>();
    }

    private void Start()
    {
        _stateMachine.ChangeState(GameStateEnum.Home);
    }
}
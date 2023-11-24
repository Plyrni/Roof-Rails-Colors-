using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // -- Public
    public PlayerMovement MovementComponent => _movement ??= GetComponent<PlayerMovement>();
    public RodCutable Rod => _rod;
    public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
    public PlayerRailSliding RailSliding => _railSliding ??= GetComponent<PlayerRailSliding>();
    public TeamColorManager TeamColorManager => _teamColorManager ??= GetComponent<TeamColorManager>();
    
    // -- Protected
    protected TeamColorElement teamColorElement => _teamColorElement ??= GetComponent<TeamColorElement>();
    
    // -- Privates
    [SerializeField] private float _baseRodeSize;
    [SerializeField] private RodCutable _rod;
    private PlayerMovement _movement;
    private Rigidbody _rigidbody;
    private PlayerRailSliding _railSliding;
    private TeamColorElement _teamColorElement;
    private TeamColorManager _teamColorManager;

    private void Awake()
    {
        Game.OnChangeState.AddListener(OnChangeState);
        Game.Player.TeamColorManager.onCurrentTeamColorChange.AddListener(OnTeamColorChange);
        Reset();
    }

    public void Reset()
    {
        Rigidbody.MovePosition(Vector3.zero + Vector3.up * 0.01f);
        Rigidbody.MoveRotation(Quaternion.identity);
        MovementComponent.Reset();
        RailSliding.Reset();
        _rod.SetSize(_baseRodeSize);
    }

    private void OnChangeState(GameState newState)
    {
        if (newState == GameState.Lose)
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnTeamColorChange(TeamColor newTeamColor)
    {
        teamColorElement.SetTeam(newTeamColor);
    }

    private void OnDestroy()
    {
        Game.OnChangeState.RemoveListener(OnChangeState);
        if (TeamColorManager != null)
            TeamColorManager.onCurrentTeamColorChange.RemoveListener(OnTeamColorChange);
    }
}
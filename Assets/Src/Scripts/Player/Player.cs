using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    // -- Public
    public PlayerMovement MovementComponent => _movement ??= GetComponent<PlayerMovement>();
    public ScaleCutable Blade => blade;
    public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
    public PlayerRailSliding RailSliding => _railSliding ??= GetComponent<PlayerRailSliding>();
    public TeamColorManager TeamColorManager => _teamColorManager ??= GetComponent<TeamColorManager>();
    public GameRegion Region => _currentRegion;

    // -- Protected
    protected TeamColorElement teamColorElement => _teamColorElement;

    // -- Privates
    [SerializeField] private float _baseRodeSize;
    [SerializeField] private ScaleCutable blade;
    [SerializeField] private TeamColorElement _teamColorElement;
    private PlayerMovement _movement;
    private Rigidbody _rigidbody;
    private PlayerRailSliding _railSliding;
    private TeamColorManager _teamColorManager;
    private GameRegion _currentRegion;


    public void Reset()
    {
        Rigidbody.MovePosition(Vector3.zero + Vector3.up * 0.05f);
        Rigidbody.MoveRotation(Quaternion.identity);
        MovementComponent.Reset();
        RailSliding.Reset();
        blade.gameObject.SetActive(true);
        blade.SetSize(_baseRodeSize);
        SetRegion(GameRegion.NONE);
    }

    public void Kill()
    {
        Rigidbody.constraints = RigidbodyConstraints.None;

        GameObject bladeCopy = Instantiate(blade.gameObject, blade.transform.position, blade.transform.rotation,
            Game.MapTransform);
        bladeCopy.AddComponent<Rigidbody>();
        bladeCopy.GetComponent<PlayerRodPositionner>().enabled = false;

        blade.gameObject.SetActive(false);

        MovementComponent.DisableInputs(Mathf.Infinity);
    }

    public void SetRegion(GameRegion newRegion)
    {
        _currentRegion = newRegion;
    }


    private void Awake()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnChangeState);
        Game.Player.TeamColorManager.onCurrentTeamColorChange.AddListener(OnTeamColorChange);
        Reset();
    }

    private void OnChangeState(GameStateEnum newStateEnum)
    {
        if (newStateEnum == GameStateEnum.Lose)
        {
            Kill();
        }
    }

    private void OnTeamColorChange(TeamColor newTeamColor)
    {
        teamColorElement.SetTeam(newTeamColor);
    }

    private void OnDestroy()
    {
        Game.StateMachine.OnStateChanged.RemoveListener(OnChangeState);
        if (TeamColorManager != null)
            TeamColorManager.onCurrentTeamColorChange.RemoveListener(OnTeamColorChange);
    }
}
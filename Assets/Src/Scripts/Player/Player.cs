using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // -- Public
    public PlayerMovement MovementComponent => _movement ??= GetComponent<PlayerMovement>();
    public ScaleCutable Blade => blade;
    public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
    public PlayerRailSliding RailSliding => _railSliding ??= GetComponent<PlayerRailSliding>();
    public TeamColorManager TeamColorManager => _teamColorManager ??= GetComponent<TeamColorManager>();
    public Character Character => _character;
    public GameRegion Region => _currentRegion;
    public UnityEvent onKill;
    public bool IsKilled => _isKilled;

    // -- Protected
    protected TeamColorElement teamColorElement => _teamColorElement;

    // -- Privates
    [SerializeField] private float _baseRodeSize;
    [SerializeField] private ScaleCutable blade;
    [SerializeField] private TeamColorElement _teamColorElement;
    [SerializeField] private Character _character;
    [SerializeField] private ScaleCutable prefabBlade;

    private PlayerMovement _movement;
    private Rigidbody _rigidbody;
    private PlayerRailSliding _railSliding;
    private TeamColorManager _teamColorManager;
    private GameRegion _currentRegion;
    private bool _isKilled = false;

    public void Reset()
    {
        Rigidbody.MovePosition(Vector3.zero + Vector3.up * 0.05f);
        Rigidbody.MoveRotation(Quaternion.identity);
        MovementComponent.Reset();
        RailSliding.Reset();
        blade.gameObject.SetActive(true);
        blade.SetLength(_baseRodeSize);
        SetRegion(GameRegion.NONE);
        _isKilled = false;
        Character.EnableMatNoFricton(false);
    }

    public void Kill()
    {
        if (_isKilled)
        {
            return;
        }

        _isKilled = true;

        //Rigidbody.constraints = RigidbodyConstraints.None;
        blade.gameObject.SetActive(false);
        MovementComponent.DisableInputs(Mathf.Infinity);
        Character.EnableMatNoFricton(true);
        //Character.EnableRagdoll();
        //Game.CameraManager.SetCurrentCamTarget(Character.RigidRagdoll.transform);
        
        ScaleCutable bladeCopy = Instantiate(prefabBlade, blade.transform.position, blade.transform.rotation,
            Game.Map.transform);
        bladeCopy.gameObject.AddComponent<Rigidbody>();
        bladeCopy.gameObject.GetComponent<PlayerBladePositionner>().enabled = false;
        bladeCopy.SetLength(blade.CurrentLength);
        bladeCopy.GetComponentInChildren<TeamColorElement>().SetTeam(_teamColorManager.CurrentTeamColor);
        onKill?.Invoke();
    }

    public void SetRegion(GameRegion newRegion)
    {
        _currentRegion = newRegion;
    }


    private void Awake()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnChangeState);
        Game.Player.TeamColorManager.onCurrentTeamColorChange.AddListener(OnTeamColorChange);
        blade.onCut.AddListener(OnCutBlade);
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

    private void OnCutBlade()
    {
    }

    private void OnDestroy()
    {
        blade.onCut.RemoveListener(OnCutBlade);
        onKill.RemoveAllListeners();
        Game.StateMachine.OnStateChanged.RemoveListener(OnChangeState);
        if (TeamColorManager != null)
            TeamColorManager.onCurrentTeamColorChange.RemoveListener(OnTeamColorChange);
    }
}
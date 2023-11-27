using System;
using UnityEngine;
using DG.Tweening;

public class Pickable_SizeUp : ItemPickable
{
    [SerializeField] private float sizeToAdd = 0.2f;
    [SerializeField] private ParticleSystem vfxPickup;
    [SerializeField] private TeamColorElement _teamColorElement;
    [SerializeField] private GameObject prefabBlade;

    private void Awake()
    {
        TeamColorManager playerTeam = Game.Player.TeamColorManager;

        _teamColorElement = GetComponent<TeamColorElement>();
        _teamColorElement.SetTeam(playerTeam.CurrentTeamColor);
        playerTeam.onCurrentTeamColorChange.AddListener(OnPlayerTeamColorChange);
    }

    private Tween _tweenPickupFeedback = null;
    private Transform _targetBlade = null;
    private float scaleXFeedback;

    protected override void OnValidColliderDetected(Collider other)
    {
        Player player = Game.Player;
        player.Blade.AddLength(sizeToAdd);

        ParticleSystem vfx = Instantiate(vfxPickup, this.transform.position, vfxPickup.transform.rotation,
            Game.Instance.transform);
        ParticleSystemRenderer rendererVFX = vfx.GetComponent<ParticleSystemRenderer>();
        rendererVFX.material = Game.DataManager.TeamColorData.GetTeamMaterial(_teamColorElement.Team);
        
        //Destroy(gameObject);
        this.gameObject.SetActive(false);

        _targetBlade = player.Blade.transform;
        FeedbackPickup();
    }

    private void OnPlayerTeamColorChange(TeamColor teamColor)
    {
        _teamColorElement.SetTeam(teamColor);
    }

    private void FeedbackPickup()
    {
        GameObject blade = Instantiate(prefabBlade, _targetBlade.position, _targetBlade.rotation, _targetBlade.parent);
        blade.transform.localScale *= 0.05f;
        Vector3 newScale = blade.transform.localScale;
        newScale.x = 0;
        scaleXFeedback = 0;
        _tweenPickupFeedback = DOTween
            .To(() => scaleXFeedback,
                x => scaleXFeedback = x,
                _targetBlade.localScale.x,
                0.2f).OnUpdate(
                () =>
                {
                    var localScaleTarget = _targetBlade.localScale;
                    blade.transform.localScale =
                        new Vector3(scaleXFeedback, localScaleTarget.y, localScaleTarget.z);
                }).OnComplete(() =>
            {
                Destroy(blade.gameObject);
                Destroy(gameObject);
            });
    }
    
    private void OnDestroy()
    {
        TeamColorManager playerTeam = Game.Player.TeamColorManager;
        if (playerTeam != null)
            playerTeam.onCurrentTeamColorChange.RemoveListener(OnPlayerTeamColorChange);
    }
}
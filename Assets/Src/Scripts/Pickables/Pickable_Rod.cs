using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Rod : ItemPickable
{
    [SerializeField] private float sizeToAdd = 0.2f;
    [SerializeField] private ParticleSystem vfxPickup;
    [SerializeField] private TeamColorElement _teamColorElement;

    private void Awake()
    {
        TeamColorManager playerTeam = Game.Player.TeamColorManager;
        
        _teamColorElement = GetComponent<TeamColorElement>();
        _teamColorElement.SetTeam(playerTeam.CurrentTeamColor);
        playerTeam.onCurrentTeamColorChange.AddListener(OnPlayerTeamColorChange);
    }

    protected override void OnValidColliderDetected(Collider other)
    {
        Game.Player.Blade.AddSize(sizeToAdd);

        Instantiate(vfxPickup, this.transform.position, vfxPickup.transform.rotation, Game.Instance.transform);

        Destroy(gameObject);
    }

    private void OnPlayerTeamColorChange(TeamColor teamColor)
    {
        _teamColorElement.SetTeam(teamColor);
    }

    private void OnDestroy()
    {
        TeamColorManager playerTeam = Game.Player.TeamColorManager;
        if (playerTeam != null)
            playerTeam.onCurrentTeamColorChange.RemoveListener(OnPlayerTeamColorChange);
    }
}
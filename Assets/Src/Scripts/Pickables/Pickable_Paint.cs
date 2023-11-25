using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Paint : ItemPickable
{
    [SerializeField] private TeamColor _paintColor;
    [SerializeField] private TeamColorElement _teamColorElement;
    [SerializeField] private GameObject _bucket;

    private void Start()
    {
        _teamColorElement.SetTeam(_paintColor);
    }

    protected override void OnValidColliderDetected(Collider other)
    {
        base.OnValidColliderDetected(other);
        Game.Player.TeamColorManager.SetTeamColor(_paintColor);
        _bucket.SetActive(false);
    }
}

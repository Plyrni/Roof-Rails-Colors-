using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TeamColorElement : MonoBehaviour
{
    public TeamColor Team => _team;
    [SerializeField] private TeamColor _team;

    [SerializeField] private Renderer[] renderersToColorize;
    private void Awake()
    {
        SetTeam(Team);
    }
    
    public void SetTeam(TeamColor teamColor)
    {
        _team = teamColor;
        SetColor(_team);
    }
    private void SetColor(TeamColor color)
    {
        foreach (var renderer in renderersToColorize)
        {
            renderer.material = SO_TeamColorData.GetTeamMaterial(_team);
        }
    }
}

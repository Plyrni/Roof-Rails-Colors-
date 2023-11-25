using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
            Material newMat = new Material(Game.DataManager.TeamColorData.GetTeamMaterial(_team));
            renderer.material = newMat;
        }
    }
}

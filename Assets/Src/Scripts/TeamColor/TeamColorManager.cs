using System;
using UnityEngine;
using UnityEngine.Events;

public class TeamColorManager : MonoBehaviour
{
    public TeamColor CurrentTeamColor => _currentTeamColor;
    [HideInInspector] public UnityEvent<TeamColor> onCurrentTeamColorChange = new UnityEvent<TeamColor>();
    
    private TeamColor _currentTeamColor = TeamColor.Yellow;


    public void SetTeamColor(TeamColor teamColor)
    {
        if (teamColor == _currentTeamColor)
        {
            return;
        }
        _currentTeamColor = teamColor;
        onCurrentTeamColorChange?.Invoke(teamColor);
    }

    private void OnDestroy()
    {
        onCurrentTeamColorChange.RemoveAllListeners();
    }
}
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TeamColorData", menuName = "ScriptableObjects/SO_TeamColorData", order = 1)]
public class SO_TeamColorData : ScriptableObject
{
    [SerializedDictionary("Team", "Material")] [SerializeField]
    private SerializedDictionary<TeamColor, Material> teamMatAssociation;

    public Material GetTeamMaterial(TeamColor teamColor)
    {
        return teamMatAssociation.GetValueOrDefault(teamColor);
    }
    
}
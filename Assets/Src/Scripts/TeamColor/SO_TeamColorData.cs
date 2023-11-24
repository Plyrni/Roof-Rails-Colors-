using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TeamColorData", menuName = "ScriptableObjects/SO_TeamColorData", order = 1)]
public class SO_TeamColorData : ScriptableObject
{
    public static SO_TeamColorData Instance => _instance ??= Resources.Load<SO_TeamColorData>("SO_TeamColorData"); 
    private static SO_TeamColorData _instance; 
    
    [SerializedDictionary("Team", "Material")] [SerializeField]
    private SerializedDictionary<TeamColor, Material> teamMatAssociation;

    public static Material GetTeamMaterial(TeamColor teamColor)
    {
        return Instance.teamMatAssociation.GetValueOrDefault(teamColor);
    }
    
}
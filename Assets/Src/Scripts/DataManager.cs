using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class DataManager : MonoBehaviour
{
    private const string LEVEL = "LEVEL";

    public  SO_TeamColorData TeamColorData => _teamColorData;
    [SerializeField] private SO_TeamColorData _teamColorData;
    
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL, 1);
    }
    public void IncrementLevel()
    {
        PlayerPrefs.SetInt(LEVEL, GetLevel() + 1);
    }
}
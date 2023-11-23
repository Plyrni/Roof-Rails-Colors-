using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public static Game Instance => instance;
    private static Game instance;
    
    public Player player;

    private void Awake()
    {
        instance = this;
    }
}

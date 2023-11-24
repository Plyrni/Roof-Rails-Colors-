using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            Game.Player.TeamColorManager.SetTeamColor(TeamColor.Yellow);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Game.Player.TeamColorManager.SetTeamColor(TeamColor.Blue);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Game.Player.TeamColorManager.SetTeamColor(TeamColor.Purple);
        }
    }
}

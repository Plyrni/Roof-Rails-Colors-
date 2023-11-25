using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionZone : MonoBehaviour
{
    [SerializeField] private GameRegion _region;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.Player.SetRegion(_region);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Game.Player.Region == _region) // Prevent overriding another region
            {
                Game.Player.SetRegion(GameRegion.NONE);
            }
        }
    }
}
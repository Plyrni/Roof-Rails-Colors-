using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Rod : ItemPickable
{
    [SerializeField] private float sizeToAdd = 0.2f;
    [SerializeField] private ParticleSystem vfxPickup;
    protected override void OnValidColliderDetected(Collider other)
    {
        Game.Instance.player.Rod.AddSize(sizeToAdd);

        Instantiate(vfxPickup, this.transform.position, vfxPickup.transform.rotation,Game.Instance.transform);
        
        Destroy(gameObject);
    }
}

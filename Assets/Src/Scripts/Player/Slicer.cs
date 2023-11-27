using System;
using System.Collections;
using DG.Tweening;
using EzySlice;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slicer : MonoBehaviour
{
    private Rigidbody rigid => _rigid ??= GetComponent<Rigidbody>();

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private LayerMask sliceableLayer;
    [SerializeField] private Material matSlice;
    [SerializeField] private float forceOnCut = 5f;
    [SerializeField] private Rigidbody _rigid;

    private void Start()
    {
        if (_rigid == null)
            _rigid = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startPoint.position, endPoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            SliceableItem item = hit.transform.gameObject.GetComponent<SliceableItem>();
            if (item != null && item.IsSliced == false)
            {
                if (TrySlice(hit.transform.gameObject))
                {
                    item.NotifySlice();
                }
            }
        }
    }

    public bool TrySlice(GameObject objToSlice)
    {
        SlicedHull hull = objToSlice.Slice(endPoint.position, this.transform.up);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(objToSlice, matSlice);
            GameObject lowerHull = hull.CreateLowerHull(objToSlice, matSlice);

            SetupSlicedPart(upperHull, -this.transform.up);
            SetupSlicedPart(lowerHull, this.transform.up);

            Destroy(objToSlice);
            return true;
        }

        return false;
    }

    private static int debug_nbSlicePartGenerated = 0;

    private void SetupSlicedPart(GameObject slicedPart, Vector3 bladeDirection)
    {
        debug_nbSlicePartGenerated++;
        slicedPart.name += debug_nbSlicePartGenerated;
        slicedPart.transform.parent = Game.Map.transform;
        slicedPart.layer = LayerMask.NameToLayer("SlicedPart");

        Rigidbody rigid = slicedPart.AddComponent<Rigidbody>();
        rigid.AddForce(-bladeDirection * forceOnCut, ForceMode.Impulse);
        rigid.AddForce(this.rigid.velocity / 2, ForceMode.VelocityChange);

        MeshCollider meshCollider = slicedPart.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        DelayedDestroy_ScaleDown delayDestroy = slicedPart.AddComponent<DelayedDestroy_ScaleDown>();
        delayDestroy.timeToDestroy = Random.Range(2f, 3f);
    }
}
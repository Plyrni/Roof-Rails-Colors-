using System.Collections;
using EzySlice;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private LayerMask sliceableLayer;
    [SerializeField] private Material matSlice;
    [SerializeField] private float forceOnCut = 5f;
    [SerializeField] private Rigidbody _rigid;

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startPoint.position, endPoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            Slice(hit.transform.gameObject);
        }
    }

    public void Slice(GameObject objToSlice)
    {
        SlicedHull hull = objToSlice.Slice(endPoint.position, this.transform.up);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(objToSlice, matSlice);
            GameObject lowerHull = hull.CreateLowerHull(objToSlice, matSlice);

            SetupSlicedPart(upperHull, -this.transform.up);
            SetupSlicedPart(lowerHull, this.transform.up);

            Destroy(objToSlice);
        }
    }

    private void SetupSlicedPart(GameObject slicedPart, Vector3 bladeDirection)
    {
        slicedPart.transform.parent = Game.MapTransform;
        slicedPart.layer = LayerMask.NameToLayer("SlicedPart");
        
        Rigidbody rigid = slicedPart.AddComponent<Rigidbody>();
        rigid.AddForce(-bladeDirection * forceOnCut, ForceMode.Impulse);
        rigid.AddForce(_rigid.velocity / 2, ForceMode.VelocityChange);
        
        MeshCollider meshCollider = slicedPart.AddComponent<MeshCollider>();
        meshCollider.convex = true;
    }
}
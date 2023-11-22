using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodCutable : MonoBehaviour
{
    [SerializeField] private GameObject prefabRod2m;
    private float currentLength => this.transform.localScale.y * 2f;
    private enum CutSide
    {
        Left = -1,
        Right = 1
    }

    public GameObject CutFromHitPos(Vector3 hitPos)
    {
        Vector3 posOnRod = ProjectOnRod(hitPos);
        float distFromCenter = posOnRod.magnitude;
        CutSide cutSide = ComputeCutSide(posOnRod);
        float lengthToCut = (currentLength/2) - distFromCenter;
        
        // Instanciate fake cuted rod
        GameObject fakeRod = Instantiate(prefabRod2m);
        ScaleRod(fakeRod, lengthToCut);
        // Position fake rod
        fakeRod.transform.position = this.transform.position + posOnRod + GetCutDirection(cutSide) * (lengthToCut / 2f);
        fakeRod.transform.rotation = this.transform.rotation;
        
        // Resize master rod 
        ScaleRod(this.gameObject, currentLength - lengthToCut);
        // Reposition master Rod
        this.transform.localPosition = this.transform.localPosition + -GetCutDirection(cutSide) * (lengthToCut / 2f);

        return fakeRod;
    }
    private Vector3 ProjectOnRod(Vector3 hitPos)
    {
        return Vector3.Project(hitPos, transform.up);
    }
    private CutSide ComputeCutSide(Vector3 posOnRod)
    {
        float dot = Vector3.Dot(posOnRod.normalized, transform.up);
        return dot > 0.9f ? CutSide.Right : CutSide.Left;
    }
    private void ScaleRod(GameObject rod, float length)
    {
        rod.transform.localScale = new Vector3(this.transform.localScale.x,length/2f,this.transform.localScale.z);
    }
    private Vector3 GetCutDirection(CutSide side)
    {
        return side == CutSide.Right ? this.transform.up : -this.transform.up; 
    }
}
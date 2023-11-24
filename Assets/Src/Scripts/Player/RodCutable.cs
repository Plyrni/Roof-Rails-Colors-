using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class RodCutable : MonoBehaviour
{
    [HideInInspector] public UnityEvent onCut;
    
    [SerializeField] private GameObject prefabRod2m;
    private float currentLength => this.transform.localScale.y * 2f;
    
    private enum CutSide
    {
        Left = -1,
        Right = 1
    }

    public GameObject CutFromHitPos(Vector3 hitPos)
    {
        Vector3 posOnRod = ProjectOnRod(hitPos - this.transform.position);
        float distFromCenter = posOnRod.magnitude;
        CutSide cutSide = ComputeCutSide(posOnRod);
        float lengthToCut = (currentLength/2) - distFromCenter;

        GameObject fakeRod = GenerateFakeRod(posOnRod, cutSide, lengthToCut);
        // Resize master rod 
        ScaleRod(this.gameObject, currentLength - lengthToCut);
        // Reposition master Rod
        this.transform.localPosition = this.transform.localPosition + -GetCutDirection(cutSide) * (lengthToCut / 2f);
        
        onCut?.Invoke();
        return fakeRod;
    }

    public void AddSize(float add)
    {
        SetSize(currentLength+add);
    }
    public void SetSize(float length)
    {
        ScaleRod(this.gameObject,length);
    }

    private GameObject GenerateFakeRod(Vector3 cutLocalPos,CutSide side,float length)
    {
        Transform parent = Game.Map.GetRootGameObjects()[0].transform;
        GameObject fakeRod = Instantiate(prefabRod2m,parent);
        ScaleRod(fakeRod, length);
        
        // Position fake rod
        fakeRod.transform.position = this.transform.position + cutLocalPos + GetCutDirection(side) * (length/2f);
        fakeRod.transform.rotation = this.transform.rotation;
        
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
        Vector3 localScale = this.transform.localScale;
        rod.transform.localScale = new Vector3(localScale.x,length/2f,localScale.z);
    }
    private Vector3 GetCutDirection(CutSide side)
    {
        return side == CutSide.Right ? this.transform.up : -this.transform.up; 
    }
}
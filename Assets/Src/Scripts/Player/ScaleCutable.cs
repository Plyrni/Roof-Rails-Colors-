using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ScaleCutable : MonoBehaviour
{
    [HideInInspector] public UnityEvent onCut;
    [SerializeField] private Direction axisUsedForRight;
    [SerializeField] private GameObject prefabRod2m;
    private float currentLength => this.transform.localScale.x * 2f;
    //private float currentLength => this.transform.localScale.y * 2f; // Version used when the blade whas a rod

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
        float lengthToCut = (currentLength / 2) - distFromCenter;
        
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
        SetSize(currentLength + add);
    }

    public void SetSize(float length)
    {
        ScaleRod(this.gameObject, length);
    }

    private GameObject GenerateFakeRod(Vector3 cutLocalPos, CutSide side, float length)
    {
        Transform parent = Game.MapTransform;
        GameObject fakeRod = Instantiate(prefabRod2m, parent);
        ScaleRod(fakeRod, length);

        // Position fake rod
        fakeRod.transform.position = this.transform.position + cutLocalPos + GetCutDirection(side) * (length / 2f);
        fakeRod.transform.rotation = this.transform.rotation;

        return fakeRod;
    }

    private Vector3 ProjectOnRod(Vector3 hitPos)
    {
        return Vector3.Project(hitPos, transform.GetDirection(axisUsedForRight));
    }

    private CutSide ComputeCutSide(Vector3 posOnRod)
    {
        float dot = Vector3.Dot(posOnRod.normalized, transform.GetDirection(axisUsedForRight));
        return dot > 0f ? CutSide.Right : CutSide.Left;
    }

    private void ScaleRod(GameObject rod, float length)
    {
        Vector3 localScale = this.transform.localScale;
        rod.transform.localScale = new Vector3(length / 2f, localScale.y, localScale.z);
        //rod.transform.localScale = new Vector3(localScale.x,length/2f,localScale.z); // Version used when the blade whas a rod
    }

    private Vector3 GetCutDirection(CutSide side)
    {
        return side == CutSide.Right ? transform.GetDirection(axisUsedForRight) : -transform.GetDirection(axisUsedForRight);
    }

    private void OnDestroy()
    {
        onCut.RemoveAllListeners();
    }
}
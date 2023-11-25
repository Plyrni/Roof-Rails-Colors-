using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopUpPoints : MonoBehaviour
{
    [SerializeField] public Vector3 velocityDir;
    [SerializeField] public float speed;

    [SerializeField] private TextMeshProUGUI textPoints;
    
    public void SetPoints(int points)
    {
        textPoints.text = "+" + points;
    }
    
    private void Update()
    {
        this.transform.position += velocityDir * speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
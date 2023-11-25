using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuHome : MonoBehaviour
{
    [SerializeField]private GameObject txtTouchToStart;

    private void Awake()
    {
        txtTouchToStart.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}

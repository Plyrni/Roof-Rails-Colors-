using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DelayedDestroy_ScaleDown : DelayedDestroy
{
    protected override void Start()
    {
        base.Start();
        this.transform.DOScale(0, timeToDestroy);
    }

    private void OnDestroy()
    {
        this.transform.DOKill();
    }
}

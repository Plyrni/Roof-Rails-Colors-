using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SliceableItem : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private PopUpPoints popup; 
    
    public void NotifySlice()
    {
        Game.DataManager.AddCurrency(value);
        SpawnPopUp();
    }

    private void SpawnPopUp()
    {
        PopUpPoints newPopUp = Instantiate(popup, Game.Map.transform);
        newPopUp.transform.position = this.transform.position + Vector3.up * 1f;
        
        // Make it look toward the camera
        newPopUp.transform.forward = newPopUp.transform.position - Game.CameraManager.Camera.transform.position; 
        newPopUp.SetPoints(value);
        
        // Destroy it after some time
        newPopUp.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack).SetDelay(2f).OnComplete(() => Destroy(newPopUp));
    }
}

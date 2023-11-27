using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public Camera Camera => _camera;
    public CinemachineVirtualCamera Vcam_Play => _vcam_Play;

    [SerializeField] private Camera _camera;
    [SerializeField] private CinemachineVirtualCamera _vcam_Play;
    [SerializeField] private CinemachineVirtualCamera _vcam_Play_ZoomBack1;
    [SerializeField] private CinemachineVirtualCamera _vcam_Play_ZoomBack2;
    [SerializeField] private CinemachineVirtualCamera _vcam_Home;
    [SerializeField] private float _minLengthToStartZoomBack = 10f;
    [SerializeField] private float _minLengthToZoomBackAgain = 15f;

    public void SetPlayerCam()
    {
        DisableAllCameras();
        _vcam_Play.gameObject.SetActive(true);   
    }
    public void SetHomeCam()
    {
        DisableAllCameras();
        _vcam_Home.gameObject.SetActive(true);
    }
    
    
    private void Start()
    {
        Game.Player.Blade.onSetSize.AddListener(OnPlayerBladeChangeSize);
     }
    private void OnPlayerBladeChangeSize(float newLength)
    {
        if (newLength > _minLengthToZoomBackAgain)
        {
            // Big dezoom
            DisableAllCameras();
            _vcam_Play_ZoomBack2.gameObject.SetActive(true);
        }
        else if (newLength > _minLengthToStartZoomBack)
        {
            // Dezoom
            DisableAllCameras();
            _vcam_Play_ZoomBack1.gameObject.SetActive(true);
        }
        else
        {
            // Normal zoom
            DisableAllCameras();
            _vcam_Play.gameObject.SetActive(true);
        }
    }
    private void DisableAllCameras()
    {
        _vcam_Play.gameObject.SetActive(false);
        _vcam_Play_ZoomBack1.gameObject.SetActive(false);
        _vcam_Play_ZoomBack2.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        Game.Player.Blade.onSetSize.RemoveListener(OnPlayerBladeChangeSize);
    }
}
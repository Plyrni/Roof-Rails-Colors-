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
    [SerializeField] private float _minLengthToStartZoomBack = 10f;
    [SerializeField] private float _minLengthToZoomBackAgain = 15f;

    private void Start()
    {
        Game.Player.Blade.onSetSize.AddListener(OnPlayerBladeChangeSize);
     }

    private void OnPlayerBladeChangeSize(float newLength)
    {
        if (newLength > _minLengthToZoomBackAgain)
        {
            // Big dezoom
            _vcam_Play.gameObject.SetActive(false);
            _vcam_Play_ZoomBack1.gameObject.SetActive(false);
            _vcam_Play_ZoomBack2.gameObject.SetActive(true);
        }
        else if (newLength > _minLengthToStartZoomBack)
        {
            // Dezoom
            _vcam_Play.gameObject.SetActive(false);
            _vcam_Play_ZoomBack1.gameObject.SetActive(true);
        }
        else
        {
            // Normal zoom
            _vcam_Play.gameObject.SetActive(true);
            _vcam_Play_ZoomBack1.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Game.Player.Blade.onSetSize.RemoveListener(OnPlayerBladeChangeSize);
    }
}
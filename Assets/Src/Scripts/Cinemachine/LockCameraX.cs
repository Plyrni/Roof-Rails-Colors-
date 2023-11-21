using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// 

[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraX : CinemachineExtension
{
    [Tooltip("Lock the camera's x position to this value")]
    public float m_XPosition = 10;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = m_XPosition;
            state.RawPosition = pos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLose : GameState
{
    public override void Init()
    {
        base.Init();
        Game.Player.Character.onCreateRagdoll.AddListener(OnPlayerCreateRagdoll);
    }


    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override void OnLateUpdate(float deltaTime)
    {
        base.OnLateUpdate(deltaTime);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void OnPlayerCreateRagdoll(GameObject ragdoll)
    {
        //Game.CameraManager.SetCurrentCamTarget(ragdoll.GetComponentInChildren<Rigidbody>().transform);
    }

    ~StateLose()
    {
        Game.Player.Character.onCreateRagdoll.RemoveListener(OnPlayerCreateRagdoll);
    }
}
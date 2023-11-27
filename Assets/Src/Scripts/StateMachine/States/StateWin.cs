using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWin : GameState
{
    private DataManager dataManager = null;

    public override void Init()
    {
        base.Init();
        Game.Player.Character.onCreateRagdoll.AddListener(OnPlayerCreateRagdoll);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (dataManager == null)
            dataManager = Game.DataManager;

        dataManager.IncrementLevel();
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
        
        // Add multiplicated coins
        float totalGains = dataManager.NbCurrencyGainedDuringLevel * dataManager.WinMultiplier;
        dataManager.AddCurrency(Mathf.CeilToInt(totalGains - dataManager.NbCurrencyGainedDuringLevel));
    }

    private void OnPlayerCreateRagdoll(GameObject ragdoll)
    {
        //Game.CameraManager.SetCurrentCamTarget(ragdoll.GetComponentInChildren<Rigidbody>().transform);
    }

    ~StateWin()
    {
        Game.Player.Character.onCreateRagdoll.RemoveListener(OnPlayerCreateRagdoll);
    }
}
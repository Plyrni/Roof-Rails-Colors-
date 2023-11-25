using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-2)]
public class DataManager : MonoBehaviour
{
    private const string LEVEL = "LEVEL";
    private const string SOFT_CURRENCY = "SOFT_CURRENCY";

    [HideInInspector] public UnityEvent<int> onAddCurrency;
    public SO_TeamColorData TeamColorData => _teamColorData;

    [SerializeField] private SO_TeamColorData _teamColorData;
    private int nbCurrencyGainedDuringLevel = 0;

    private void Start()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnGameStateChange);
    }


    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL, 1);
    }

    public void IncrementLevel()
    {
        PlayerPrefs.SetInt(LEVEL, GetLevel() + 1);
    }

    #region Currency

    public void AddCurrency(int nb)
    {
        if (nb <= 0)
        {
            Debug.LogWarning("Cant add negative numbers");
        }

        SetCurrency(GetCurrency() + nb);
        onAddCurrency?.Invoke(nb);
    }

    public void RemoveCurrency(int nb)
    {
        if (nb >= 0)
        {
            Debug.LogWarning("Cant remove negative numbers");
        }

        SetCurrency(GetCurrency() - nb);
    }

    public int GetCurrency()
    {
        return PlayerPrefs.GetInt(SOFT_CURRENCY, 0);
    }

    public void SetCurrency(int newAmount)
    {
        PlayerPrefs.SetInt(SOFT_CURRENCY, newAmount);
    }

    private void OnAddCurrency(int amountAdded)
    {
        nbCurrencyGainedDuringLevel += amountAdded;
    }

    #endregion


    private void OnGameStateChange(GameStateEnum newState)
    {
        if (newState == GameStateEnum.Home)
        {
            nbCurrencyGainedDuringLevel = 0;
        }
    }
}
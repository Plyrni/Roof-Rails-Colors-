using System;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-2)]
public class DataManager : MonoBehaviour
{
    private const string LEVEL = "LEVEL";
    private const string SOFT_CURRENCY = "SOFT_CURRENCY";

    [HideInInspector] public UnityEvent<int> onAddCurrency;
    public SO_TeamColorData TeamColorData => _teamColorData;
    public int NbCurrencyGainedDuringLevel => _nbCurrencyGainedDuringLevel;
    public float WinMultiplier => _winMultiplier;

    [SerializeField] private SO_TeamColorData _teamColorData;
    private int _nbCurrencyGainedDuringLevel = 0; // It would have been better to put it inside "Map.cs" but i stumbled on an "android only" bug preventing me to access it somehow. I'll dig that later
    private float _winMultiplier = 1; // Same here

    private void Start()
    {
        Game.StateMachine.OnStateChanged.AddListener(OnGameStateChange);
        onAddCurrency.AddListener(OnAddCurrency);
    }

    public void SetWinMultiplier(float winMultiplier)
    {
        _winMultiplier = winMultiplier;
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
        _nbCurrencyGainedDuringLevel += amountAdded;
    }

    #endregion


    private void OnGameStateChange(GameStateEnum newState)
    {
        if (newState == GameStateEnum.Home)
        {
            _nbCurrencyGainedDuringLevel = 0;
            _winMultiplier = 1;
        }
    }

    private void OnDestroy()
    {
        onAddCurrency.RemoveListener(OnAddCurrency);
        Game.StateMachine.OnStateChanged.RemoveListener(OnGameStateChange);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIWinReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNbGained;
    [SerializeField] private TextMeshProUGUI txtMultiplier;
    [SerializeField] private TextMeshProUGUI txtTotal;
    [SerializeField] private RectTransform line;
    [SerializeField] private bool _shouldProcessMultiplier = true;

    private void OnEnable()
    {
        DataManager dataManager = Game.DataManager;

        ScaleDownAll();
        int gains = dataManager.NbCurrencyGainedDuringLevel;
        txtNbGained.text = gains.ToString();
        txtNbGained.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(0.5f);

        if (_shouldProcessMultiplier)
        {
            float multiplier = dataManager.WinMultiplier;
            txtMultiplier.text = 'X' + multiplier.ToString("0");
            txtMultiplier.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(1f);

            line.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(1.25f);

            float total = gains * multiplier;
            txtTotal.text = total.ToString("0");
            txtTotal.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(1.4f);
        }
    }

    private void ScaleDownAll()
    {
        txtNbGained.transform.localScale = Vector3.zero;
        txtMultiplier.transform.localScale = Vector3.zero;
        txtTotal.transform.localScale = Vector3.zero;
        line.transform.localScale = Vector3.zero;
    }
}
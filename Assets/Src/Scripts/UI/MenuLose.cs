using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuLose : MonoBehaviour
{
    [FormerlySerializedAs("btnNext")] [SerializeField] private Button btnRetry;

    private void Awake()
    {
        btnRetry.onClick.AddListener(OnClickBtnRetry);
    }

    private void OnClickBtnRetry()
    {
        Game.ChangeState(GameStateEnum.Home);
    }

    private void OnDestroy()
    {
        btnRetry.onClick.RemoveListener(OnClickBtnRetry);
    }
}

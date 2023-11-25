using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlay : MonoBehaviour
{
    [SerializeField] private Button btnRetry;
    void Start()
    {
        btnRetry.onClick.AddListener(OnClickRetry);
    }

    private void OnClickRetry()
    {
        Game.StateMachine.ChangeState(GameStateEnum.Home);
    }
}

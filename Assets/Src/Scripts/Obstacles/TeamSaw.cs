using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeamSaw : Saw
{
    [SerializeField] private TeamColorElement _teamColorElement;
    [SerializeField] private Transform saw;
    [SerializeField] private Transform posSawHidden;
    [SerializeField] private GameObject playerCollider;

    protected override void Awake()
    {
        base.Awake();
        Game.Player.TeamColorManager.onCurrentTeamColorChange.AddListener(OnPlayerChangeColor);
        OnPlayerChangeColor(Game.Player.TeamColorManager.CurrentTeamColor);
    }

    private void OnPlayerChangeColor(TeamColor newTeamColor)
    {
        if (newTeamColor == _teamColorElement.Team)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    protected override bool IsValidCollider(Collider other)
    {
        if (base.IsValidCollider(other))
        {
            TeamColorElement colliderTeamElement = other.GetComponent<TeamColorElement>();
            if (colliderTeamElement != null && colliderTeamElement.Team != _teamColorElement.Team)
            {
                return true;
            }
        }

        return false;
    }

    private void Show()
    {
        saw.transform.DOKill();
        saw.transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
        playerCollider.SetActive(true);
        EnableRotation(true);
    }

    private void Hide()
    {
        saw.transform.DOKill();
        saw.transform.DOLocalMove(posSawHidden.localPosition, 0.3f).SetEase(Ease.InBack);
        playerCollider.SetActive(false);
        EnableRotation(false);
    }

    private void OnDestroy()
    {
        saw.transform.DOKill();
        Game.Player.TeamColorManager.onCurrentTeamColorChange.RemoveListener(OnPlayerChangeColor);
    }
}
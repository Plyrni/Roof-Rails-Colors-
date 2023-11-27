using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
	private Animator _animator;
	[SerializeField] private Transform _ikPosHandLeft;
	[SerializeField] private Transform _ikPosHandRight;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		
	}

	private void OnAnimatorIK(int layerIndex)
	{
		_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
		_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
		
		_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
		_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
		
		_animator.SetIKPosition(AvatarIKGoal.LeftHand, _ikPosHandLeft.position);
		_animator.SetIKRotation(AvatarIKGoal.LeftHand, _ikPosHandLeft.rotation);
		
		_animator.SetIKPosition(AvatarIKGoal.RightHand, _ikPosHandRight.position);
		_animator.SetIKRotation(AvatarIKGoal.RightHand, _ikPosHandRight.rotation);
	}
}

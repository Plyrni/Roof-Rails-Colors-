using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public UnityEvent<bool> onGroundedStateChange;
    public bool IsGrounded => _isGrounded;

    [SerializeField] private float _sensivity;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    
    private RigidbodyConstraints _constraintsOnGround = RigidbodyConstraints.FreezeRotation;
    private RigidbodyConstraints _constraintsOnFall =
        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    private float _horizontalMoveAccumulated = 0;
    private bool _isGrounded;
    private float _currentSpeed;
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        LeanTouch.OnGesture += ManageInputs;

        _currentSpeed = _normalSpeed;
    }

    private void FixedUpdate()
    {
        MoveForward();
        MoveHorizontally();

        UpdateIsGrounded();
        UpdateConstraints();
    }


    private void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger.IsActive && !finger.StartedOverGui)
        {
            _horizontalMoveAccumulated += finger.ScaledDelta.x * _sensivity;
        }
    }

    private void MoveForward()
    {
        Vector3 currentVelocity = _rigid.velocity;
        currentVelocity.z = _currentSpeed * Time.fixedDeltaTime;
        _rigid.velocity = currentVelocity;
    }

    private void MoveHorizontally()
    {
        Vector3 currentVelocity = _rigid.velocity;
        currentVelocity.x = _horizontalMoveAccumulated;
        _rigid.velocity = currentVelocity;
        _horizontalMoveAccumulated = 0;
    }

    private void UpdateIsGrounded()
    {
        Ray rayDown = new Ray(this.transform.position + this.transform.up * 0.01f, -this.transform.up);
        bool newGroundedState = Physics.Raycast(rayDown, 0.02f);

        if (newGroundedState != _isGrounded)
        {
            _isGrounded = newGroundedState;
            onGroundedStateChange?.Invoke(_isGrounded);
        }
    }

    private void UpdateConstraints()
    {
        _rigid.constraints = _isGrounded ? _constraintsOnGround : _constraintsOnFall;
    }

    private void OnDestroy()
    {
        onGroundedStateChange.RemoveAllListeners();
    }
}
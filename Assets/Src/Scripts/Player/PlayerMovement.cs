using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _sensivity;
    [SerializeField] private float _normalSpeed;
    private RigidbodyConstraints constraintsOnGround = RigidbodyConstraints.FreezeRotation;
    private RigidbodyConstraints constraintsNotOnGround = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

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
            Debug.Log(_horizontalMoveAccumulated);
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
        Ray rayDown = new Ray(this.transform.position + Vector3.up * 0.01f, Vector3.down);
        _isGrounded = Physics.Raycast(rayDown, 0.02f);
    }
    private void UpdateConstraints()
    {
        _rigid.constraints = _isGrounded ? constraintsOnGround : constraintsNotOnGround;
    }
}
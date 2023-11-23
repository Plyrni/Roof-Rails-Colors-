using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public UnityEvent<bool> onGroundedStateChange;
    public bool IsGrounded => _isGrounded;

    [SerializeField] private float _sensivity;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;

    private float _horizontalMoveAccumulated = 0;
    private bool _isGrounded;
    private float _currentSpeed;
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        LeanTouch.OnGesture += ManageInputs;
        Game.OnChangeState.AddListener(OnChangeState);
        
        _currentSpeed = _normalSpeed;
    }

    private void OnChangeState(GameState newState)
    {
        if (newState != GameState.Playing)
        {
            if (_isGrounded)
            {
                _rigid.velocity = Vector3.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Game.State != GameState.Playing)
        {
            return;
        }

        UpdateIsGrounded();
        
        Vector3 velocity = ComputeMoveVelocity();
        
        // Reset gravity force if on ground
        if (_isGrounded && _rigid.velocity.x < 0)
        {
            velocity.y = 0;
        }
        
        // Apply velocity
        _rigid.velocity = velocity;
        
        ClampPlayerPos();
    }

    private void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger.IsActive && !finger.StartedOverGui)
        {
            _horizontalMoveAccumulated += finger.ScaledDelta.x * _sensivity;
        }
    }

    private Vector3 ComputeMoveVelocity()
    {
        Vector3 tempVelocity = _rigid.velocity;

        // Forward
        tempVelocity.z = _currentSpeed * Time.fixedDeltaTime;

        // Horizontaly
        tempVelocity.x = _horizontalMoveAccumulated;
        _horizontalMoveAccumulated = 0;

        return tempVelocity;
    }

    Vector3 PredictPosition(Rigidbody rb, float timeAhead)
    {
        // Predict future position based on current velocity and time ahead
        return rb.position + rb.velocity * timeAhead;
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

    private void ClampPlayerPos()
    {
        float posX = PredictPosition(_rigid, Time.fixedDeltaTime).x;
        if (posX < xMin || posX > xMax)
        {
            float newPosX = posX < 0 ? xMin : xMax;
            Vector3 newVel = _rigid.velocity;
            newVel.x = 0;
            _rigid.velocity = newVel;
            this.transform.position = new Vector3(newPosX, this.transform.position.y, this.transform.position.z);
        }
    }

    private void OnDestroy()
    {
        onGroundedStateChange.RemoveAllListeners();
        Game.OnChangeState.RemoveListener(OnChangeState);
    }
}
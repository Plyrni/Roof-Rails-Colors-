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
    [HideInInspector] public UnityEvent<bool> onIsMovingChange;
    public bool IsGrounded => _isGrounded;
    public bool IsMoving => _isMoving;

    [SerializeField] private float _sensivity;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private CapsuleCollider _capsule;
    [SerializeField] private GameObject vfxLand;

    private Rigidbody rigid => _rigid ??= GetComponent<Rigidbody>();
    private float _horizontalMoveAccumulated = 0;
    private bool _isGrounded;
    private float _currentSpeed;
    private Rigidbody _rigid;
    private Vector3 _bumpVector;
    private bool _isUserInputEnabled => _durationNoInputRemaining <= 0;
    private float _durationNoInputRemaining;
    private float _fallStartY = 0;
    private bool _isMoving;

    public void Reset()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        _horizontalMoveAccumulated = 0f;
        EnableInputs();
        Application.targetFrameRate = -1;
        _fallStartY = this.transform.position.y;
    }

    /// <summary></summary>
    /// <param name="direction">Is normalized internaly</param>
    /// <param name="force"></param>
    public void Bump(Vector3 direction, float force)
    {
        _bumpVector += direction.normalized * force;
    }

    public void SetMovementBounds(float minX, float maxX)
    {
        this.xMin = minX;
        this.xMax = maxX;
    }

    public void EnableInputs()
    {
        _durationNoInputRemaining = 0;
    }

    public void DisableInputs(float duration)
    {
        if (duration < _durationNoInputRemaining)
        {
            return;
        }

        _durationNoInputRemaining = duration;
    }

    private void Awake()
    {
        LeanTouch.OnGesture += ManageInputs;
        Game.StateMachine.OnStateChanged.AddListener(OnChangeState);
        onGroundedStateChange.AddListener(UpdateFall);

        _currentSpeed = _normalSpeed;
    }

    private void OnChangeState(GameStateEnum newStateEnum)
    {
        if (newStateEnum != GameStateEnum.Playing)
        {
            if (_isGrounded)
            {
                rigid.velocity = Vector3.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Game.State == GameStateEnum.Playing)
        {
            UpdateTimerDisabledInput();
            UpdateIsGrounded();

            Vector3 velocity = ComputeMoveVelocity();

            // Reset gravity force if on ground
            if (_isGrounded && rigid.velocity.x < 0)
            {
                velocity.y = 0;
            }

            // Apply velocity
            rigid.velocity = velocity;

            if (_isGrounded)
            {
                ClampPlayerPos();
            }
        }

        UpdateIsMoving();
    }

    private void ManageInputs(List<LeanFinger> fingers)
    {
        LeanFinger finger = fingers[0];
        if (finger.IsActive)
        {
            if (Game.State == GameStateEnum.Playing)
            {
                _horizontalMoveAccumulated += (finger.ScreenDelta.x / Screen.width) * _sensivity;
            }
        }
    }

    private Vector3 ComputeMoveVelocity()
    {
        Vector3 tempVelocity = rigid.velocity;

        // Forward
        tempVelocity.z = _currentSpeed * Time.fixedDeltaTime;

        // Horizontal player input
        if (_isUserInputEnabled)
        {
            tempVelocity.x =
                _horizontalMoveAccumulated; // Override velocity X with swipe input. Nice responsive movement.
        }

        _horizontalMoveAccumulated = 0;

        // Apply potential bump
        tempVelocity += _bumpVector;
        _bumpVector = Vector3.zero;

        // Computation complete
        return tempVelocity;
    }

    Vector3 PredictPosition(Rigidbody rb, float timeAhead)
    {
        // Predict future position based on current velocity and time ahead
        return rb.position + rb.velocity * timeAhead;
    }

    private void UpdateIsMoving()
    {
        bool tempIsMoving = _rigid.velocity.z > 1f;

        if (tempIsMoving != _isMoving)
        {
            _isMoving = tempIsMoving;
            onIsMovingChange.Invoke(_isMoving);
        }
    }

    private void UpdateIsGrounded()
    {
        Ray rayDown = new Ray(this.transform.position + this.transform.up * 0.01f, -this.transform.up);
        bool newGroundedState = Physics.Raycast(rayDown, out RaycastHit hit, 0.03f, ~0,
            QueryTriggerInteraction.Ignore);

        if (newGroundedState != _isGrounded)
        {
            _isGrounded = newGroundedState;
            onGroundedStateChange?.Invoke(_isGrounded);
        }
    }

    private void UpdateFall(bool isGrounded)
    {
        if (isGrounded)
        {
            float fallDist = this.transform.position.y - _fallStartY;
            if (fallDist < -1f)
            {
                GameObject landVFX = Instantiate(vfxLand, this.transform.position + Vector3.forward * 0.5f,
                    vfxLand.transform.rotation);
            }
        }
        else
        {
            _fallStartY = this.transform.position.y;
        }
    }

    private void ClampPlayerPos()
    {
        float posX = PredictPosition(rigid, Time.fixedDeltaTime).x;
        if (posX < xMin || posX > xMax)
        {
            float newPosX = posX > xMax ? xMax : xMin;
            Vector3 newVel = rigid.velocity;
            newVel.x = 0;
            rigid.velocity = newVel;
            this.transform.position = new Vector3(newPosX, this.transform.position.y, this.transform.position.z);
        }
    }

    private void UpdateTimerDisabledInput()
    {
        if (_isUserInputEnabled == false)
        {
            _durationNoInputRemaining -= Time.fixedDeltaTime;
        }
    }


    private void OnDestroy()
    {
        onGroundedStateChange.RemoveAllListeners();
        onIsMovingChange.RemoveAllListeners();
        Game.StateMachine.OnStateChanged.RemoveListener(OnChangeState);
    }
}
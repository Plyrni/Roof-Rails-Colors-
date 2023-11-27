using DG.Tweening;
using UnityEngine;


public class PlayerBladePositionner : MonoBehaviour
{
    [SerializeField] private float _waitTimeToCenterRod = 0.5f;
    [SerializeField] private float _lerpSpeed = 0.5f;
    private float _timeSinceCut;
    private bool _isCentering;
    private bool _isTimerRunning => _timeSinceCut < _waitTimeToCenterRod;
    private Player _playerOwner;
    private ScaleCutable _blade;
    private Vector3 _baseLocalPos;
    private Vector3 _baseHandleLocalPos;
    private Tween _tweenLocalPosZ = null;
    [SerializeField] private Transform _handle;

    private void Awake()
    {
        _playerOwner = GetComponentInParent<Player>();
        _blade = GetComponent<ScaleCutable>();
        _timeSinceCut = Mathf.Infinity;
        _baseLocalPos = this.transform.localPosition;
        if (_handle != null)
            _baseHandleLocalPos = _handle.localPosition;
    }

    private void Start()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.AddListener(OnPlayerGroundStateChange);
        _blade.onCut.AddListener(OnBladeCut);
    }


    private void Update()
    {
        if (_isTimerRunning)
        {
            _timeSinceCut += Time.deltaTime;
            if (_timeSinceCut >= _waitTimeToCenterRod)
            {
                _isCentering = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_tweenLocalPosZ != null)
        {
            _tweenLocalPosZ.ManualUpdate(Time.fixedDeltaTime, Time.unscaledTime);
        }

        if (_isCentering)
        {
            Vector3 currentLocalPos = _blade.transform.localPosition;
            float blend = Mathf.Pow(0.5f, Time.fixedDeltaTime * _lerpSpeed);
            float newLocalX = Mathf.Lerp(currentLocalPos.x, 0, blend);

            if ((newLocalX <= 0 && newLocalX > -0.01f) || (newLocalX > 0 && newLocalX < 0.01f))
            {
                _isCentering = false;
                _timeSinceCut = _waitTimeToCenterRod;
                newLocalX = 0f;
            }

            Vector3 newRodLocalPos = currentLocalPos;
            newRodLocalPos.x = newLocalX;

            _blade.transform.localPosition = newRodLocalPos;
        }
    }

    private void OnPlayerGroundStateChange(bool newValue)
    {
        if (newValue == true)
        {
            OnPlayerLand();
        }
        else
        {
            OnPlayerFall();
        }
    }

    private void OnPlayerFall()
    {
    }

    private void OnPlayerLand()
    {
    }

    private void OnBladeCut()
    {
        KnockbackFeedback();
        TryResetTimer_Centering();
    }

    private void KnockbackFeedback()
    {
        float zOffset = -0.25f;
        float duration = 0.1f;

        _tweenLocalPosZ.Kill();
        _tweenLocalPosZ = this.transform.DOLocalMoveZ(_baseLocalPos.z + zOffset, duration).SetUpdate(UpdateType.Manual)
            .SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);

        if (_handle != null)
        {
            _handle.transform.DOKill();
            _handle.transform.DOLocalMoveZ(_baseHandleLocalPos.z + zOffset, duration).SetUpdate(UpdateType.Fixed)
                .SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    private void TryResetTimer_Centering()
    {
        // Only reset if timer not running
        if (_isTimerRunning == false)
        {
            _timeSinceCut = 0;
        }
    }

    private void OnDestroy()
    {
        if (_playerOwner != null)
            _playerOwner.MovementComponent.onGroundedStateChange.RemoveListener(OnPlayerGroundStateChange);

        _blade.onCut.RemoveListener(OnBladeCut);
    }
}
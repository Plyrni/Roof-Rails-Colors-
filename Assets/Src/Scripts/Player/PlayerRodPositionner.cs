using UnityEngine;


public class PlayerRodPositionner : MonoBehaviour
{
    [SerializeField] private float _waitTimeToCenterRod = 0.5f;
    [SerializeField] private float _lerpSpeed = 0.5f;
    private float _timeSinceCut;
    private bool _isCentering;
    private bool _isTimerRunning => _timeSinceCut < _waitTimeToCenterRod;
    private Player _playerOwner;
    private ScaleCutable scale;

    private void Awake()
    {
        _playerOwner = GetComponentInParent<Player>();
        scale = GetComponent<ScaleCutable>();
        _timeSinceCut = Mathf.Infinity;
    }

    private void Start()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.AddListener(OnPlayerGroundStateChange);
        scale.onCut.AddListener(OnPlayerRodCut);
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
        if (_isCentering)
        {
            Vector3 currentLocalPos = scale.transform.localPosition;
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

            scale.transform.localPosition = newRodLocalPos;
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

    private void OnPlayerRodCut()
    {
        TryResetTimer_Centering();
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

        scale.onCut.RemoveListener(OnPlayerRodCut);
    }
}
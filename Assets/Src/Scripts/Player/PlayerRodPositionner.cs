using UnityEngine;


public class PlayerRodPositionner : MonoBehaviour
{
    [SerializeField] private float _waitTimeToCenterRod = 0.5f;
    [SerializeField] private float _lerpSpeed = 0.5f;
    private float _timeSinceCut;
    private bool _isCentering;
    private bool _isTimerRunning => _timeSinceCut < _waitTimeToCenterRod;
    private Player _playerOwner;
    private RodCutable _rod;

    private void Awake()
    {
        _playerOwner = GetComponentInParent<Player>();
        _rod = GetComponent<RodCutable>();
        _timeSinceCut = Mathf.Infinity;

    }

    private void Start()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.AddListener(OnPlayerGroundStateChange);
        _rod.onCut.AddListener(OnPlayerRodCut);
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
            Debug.LogWarning("[PlayerRodPositionner] Centering...");
            Vector3 currentLocalPos = _rod.transform.localPosition;
            float blend = Mathf.Pow(0.5f, Time.fixedDeltaTime * _lerpSpeed);
            float newLocalX = Mathf.Lerp(currentLocalPos.x, 0, blend);

            if ((currentLocalPos.x <= 0 && newLocalX > -0.01f) || (currentLocalPos.x > 0 && newLocalX < 0.01f))
            {
                _isCentering = false;
                newLocalX = 0f;
                Debug.LogWarning("[PlayerRodPositionner] Stop centering");
            }
            
            Vector3 newRodLocalPos = currentLocalPos;
            newRodLocalPos.x = newLocalX;
            
            _rod.transform.localPosition = newRodLocalPos;

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
            Debug.LogWarning("[PlayerRodPositionner] Start counter ");
        }
    }

    private void OnDestroy()
    {
        _playerOwner.MovementComponent.onGroundedStateChange.RemoveListener(OnPlayerGroundStateChange);
        _rod.onCut.RemoveListener(OnPlayerRodCut);
    }
}
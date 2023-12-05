using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private InputReader _inputReader;
    private CarController _carController;

    [Header("Car")] 
    [SerializeField] private LayerMask _whatIsGround;
    private Rigidbody _rigidbody;
    private Quaternion _targetRotation;
    private Vector3 _direction = Vector3.back;
    private Vector3 _arrowUIPos;
    [SerializeField] private float _currentMaxSpeed = 2;
    [SerializeField] private float _currentSpeed = 0;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set
        {
            _currentSpeed = value;
            if (_currentSpeed > _currentMaxSpeed)
            {
                Vector3 clampedVel = _rigidbody.velocity.normalized * _currentMaxSpeed;
                _rigidbody.velocity = clampedVel;
            }
        }
    }

    [Header("Booster")] 
    private ParticleSystem _boosterParticle;
    private bool _isGrounded = false;
    private bool _isBooster = false;
    private Coroutine _boosterCo;
    public bool IsBooster
    {
        get => _isBooster;
        set
        {
            if (_boosterParticle == null)
                _boosterParticle = transform.Find("BoostEffect").GetComponent<ParticleSystem>();
            if (value)
            {
                if (_boosterCo != null)
                    StopCoroutine(_boosterCo);
                _boosterParticle.Play();
                if (_carController.CarData)
                    _currentMaxSpeed = _carController.CarData.MaxSpeed * 1.2f;
                _isBooster = value;
            }
            else
            {
                if (_boosterCo != null)
                    StopCoroutine(_boosterCo);
                _boosterParticle.Stop();
                _boosterCo = StartCoroutine(BoosterCo());
            }
        }
    }

    [Header("Wheel")] 
    public List<WheelData> Wheel;

    [Header("Other")] 
    [SerializeField] private bool _isHome = false;

    private Coroutine activeBoosterCo;
    
    private void Awake()
    {
        if (_isHome) return;
        _inputReader.PositionEvent += Movement;
        _carController = GetComponent<CarController>();
        _rigidbody = GetComponent<Rigidbody>();
        IsBooster = false;
    }

    private void Start()
    {
        if (_isHome) return;
        IsBooster = false;
        _arrowUIPos = new Vector3(Screen.width * .5f, Screen.height * .25f);
    }

    private void FixedUpdate()
    {
        if (_isHome) return;
        if (!_carController.IsInitEnd)
            return;
        GroundCheck();
        SetRotationPoint();
        Movement();
        //WheelRotation();
    }

    private void Update()
    {
        if (_carController.CarData == null)
            return;
        
        CurrentSpeed = _rigidbody.velocity.magnitude;
        
        SpeedLimit();
    }

    private void OnDestroy()
    {
        _inputReader.PositionEvent -= Movement;
    }

    private void GroundCheck()
    {
        if (!Physics.Raycast(transform.position + new Vector3(0, 1, 1), Vector3.down, _carController.CarData.RayDistance, _whatIsGround) &&
            !Physics.Raycast(transform.position + new Vector3(0, 1, -1), Vector3.down, _carController.CarData.RayDistance, _whatIsGround))
        {
            _isGrounded = false;
        }
        else
            _isGrounded = true;
    }

    private void SetRotationPoint()
    {
        float rotationAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        _targetRotation = Quaternion.Euler(0, rotationAngle, 0);

        float rotationDifference = Mathf.Abs(transform.eulerAngles.y - rotationAngle);
        if (rotationDifference >= 180)
            rotationDifference = Mathf.Abs(rotationDifference - 360.0f);

        if (rotationDifference > 7.5f)
        {
            Vector3 clampedVel = _rigidbody.velocity.normalized * (CurrentSpeed - (rotationDifference * 0.0015f));
            //_rigidbody.velocity = clampedVel;
        }
    }

    public void ActiveBoosterForSecond(float time)
	{
        if (activeBoosterCo != null)
		{
            StopCoroutine(activeBoosterCo);
		}
        activeBoosterCo = StartCoroutine(ActiveBoosterCo(time));
	}

    private IEnumerator ActiveBoosterCo(float time)
	{
        yield return new WaitForSeconds(time);

        IsBooster = false;
	}

    private void Movement()
    {
        _carController.CurrentSpeed = CurrentSpeed;
        
        if (!_isGrounded)
            return;
        _rigidbody.AddForce(transform.forward * (_carController.CarData.Acceleration * Time.fixedDeltaTime));
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _carController.CarData.TurnSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator BoosterCo()
    {
        if (_carController.CarData == null)
        {
            _isBooster = false;
            yield break;
        }
        
        float currentTime = 0;
        float startSpeed = _currentSpeed;
        while (currentTime < .5f)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float time = currentTime / .5f;
            _currentMaxSpeed = Mathf.Lerp(_currentSpeed, _carController.CarData.MaxSpeed, time);
        }
        _isBooster = false;
    }

    private void SpeedLimit()
    {
        if (IsBooster)
            return;
        _currentMaxSpeed += Time.deltaTime * (((CurrentSpeed / _carController.CarData.MaxSpeed) * -1) + 1) * 100000;
        _currentMaxSpeed = Mathf.Clamp(_currentMaxSpeed, CurrentSpeed, CurrentSpeed + 0.5f);
        _currentMaxSpeed = Mathf.Clamp(_currentMaxSpeed, CurrentSpeed, _carController.CarData.MaxSpeed);
        if (_currentMaxSpeed > _carController.CarData.MaxSpeed)
            _currentMaxSpeed = _carController.CarData.MaxSpeed;
    }

    private void WheelRotation()
    {
        bool isRight = false;
        foreach (var wheel in Wheel)
        {
            if (wheel.Type == WheelType.Front)
            {
                if (isRight)
                    _targetRotation.y += 0;
                wheel.WheelTrm.transform.rotation = _targetRotation;
                isRight = true;
            }
        }
    }

    /// <summary>
    /// Booster을 특정 시간동안 사용하게 해주는 함수
    /// </summary>
    /// <param name="time">Booster을 사용할 시간</param>
    public void StartBooster(float time)
    {
        if (IsBooster || time < 0.2f)
            return;

        time = Mathf.Clamp(time, 0.1f, 1);
        StartCoroutine(StartBoosterCo(time));
    }

    private IEnumerator StartBoosterCo(float time)
    {
        IsBooster = true;
        yield return new WaitForSeconds(time);
        IsBooster = false;
    }

    public void CarInit()
    {
        Wheel = new List<WheelData>();
        for (int i = 0; i < 4; ++i)
        {
            Transform trm = _carController.CarVisual.GetChild(i + 1).transform;
            WheelType wheelType = i < 2 ? WheelType.Back : WheelType.Front;
            WheelData wheelData = new WheelData(wheelType, trm, trm.GetChild(0).GetComponent<ParticleSystem>());
            Wheel.Add(wheelData);
        }
    }

    private void Movement(Vector2 pos)
    {
        _direction = new Vector3(pos.x - _arrowUIPos.x, 0, pos.y - _arrowUIPos.y);
    }
}

//참고할거
//https://www.youtube.com/watch?v=pAsCXXsuB1M&t=1307s

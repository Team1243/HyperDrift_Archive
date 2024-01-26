using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private PlayerQuestCheck _playerQuestCheck;
    
    [Header("Core")]
    public CarDataListSO carDataList;
    public CarData CarData;

    [Header("Car")] 
    [HideInInspector] public bool IsInitEnd = false;
    [HideInInspector] public Transform CarVisual;
    [HideInInspector] public float HighSpeed = 0; // 퀘스트에 사용
    private float _carPositionY = 0;
    private float _currentSpeed;
    public float CurrentSpeed 
    {
        set
        {
            _currentSpeed = value;
            if (HighSpeed < _currentSpeed)
                HighSpeed = _currentSpeed;
        }
    }

    [Header("Collision")] 
    private Rigidbody _rigidbody;
    
    [Header("Drift")]
    private List<ParticleSystem> _driftParticles = new List<ParticleSystem>();
    private bool _particleShow = false;
    private Vector3 _lastPosition;
    private float _sideSlipAmount = 0;
    public float SideSlipAmount => _sideSlipAmount;

    [Header("Collider")] 
    private SphereCollider _sphereCollider;

    [Header("Component")] 
    private CarMovement _carMovement;

    [Header("Quest")]
    public double DriftTime; // 퀘스트에 사용
    private float _driftStartTime;
    public double DriftDistance;
    private double _driftStartDistance;
    public double MoveDistance; // 퀘스트에 사용
    private double _currentMoveDistance = 0;
    public double CurrentMoveDistance => _currentMoveDistance * 0.005f;
    private Vector3 _lastPos;

    [Header("Other")] 
    [SerializeField] private bool _isHome;
    private GameObject _arrow;

    private void Awake()
    {
        if (_isHome) return;
        for (int i = 0; i < 3; ++i)
            _driftParticles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        _sphereCollider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _carMovement = GetComponent<CarMovement>();
        // _playerQuestCheck = GetComponentInChildren<PlayerQuestCheck>();
        _arrow = FindObjectOfType<PlayerDirectionUI>().gameObject;
    }

    private void Start()
    {
        if (_isHome) return;
        _lastPos = transform.position;
        StartCoroutine(CarInitCo());
    }

    private void Update()
    {
        if (_isHome) return;
        SetSlideSlip();
        MoveDistanceCheck();
        /*if (IsInitEnd)
            transform.position = new Vector3(transform.position.x, _carPositionY, transform.position.z);*/
    }

    private void OnEnable()
    {
        _playerQuestCheck = GameObject.Find("QuestCheck").GetComponent<PlayerQuestCheck>();  
    }

    private IEnumerator CarInitCo()
    {
        //나중에 꼭 지워주세요 !!!!! 
        //PlayerPrefs.SetInt("car", 7);
        
        while (!CarData)
        {
            if (PlayerPrefs.HasKey("car"))
                CarData = carDataList.CarDataList[PlayerPrefs.GetInt("car")];
            else
                CarData = carDataList.CarDataList[17];
            yield return null;
        }
        /*while(!CarDataSave.Instance.IsLoaded)
            yield return null;*/
        
        CarVisual = Instantiate(CarData.CarModel, transform.position, Quaternion.Euler(0, 180, 0), transform).transform;
        _rigidbody.mass = CarData.CarMass;
        _sphereCollider.radius = CarData.ColliderRadius;
        _sphereCollider.center = CarData.ColliderCenter;
        _carMovement.CarInit();

        yield return new WaitForSeconds(2.4f);

        _carPositionY = transform.position.y;
        IsInitEnd = true;
    }

    private void SetSlideSlip()
    {
        Vector3 direction = transform.position - _lastPosition;
        Vector3 movement = transform.InverseTransformDirection(direction);
        _lastPosition = transform.position;
        if (movement == Vector3.zero)
            return;

        _sideSlipAmount = Mathf.Abs(movement.x);

        if (_sideSlipAmount > 0.5f && !_particleShow)
        {
            _driftParticles.ForEach(p => p.Play());
            if (_carMovement.Wheel != null)
                _carMovement.Wheel.ForEach(wheel => wheel.SkidParticle.Play());
            _particleShow = true;
            _driftStartTime = Time.time;
            _driftStartDistance = MoveDistance;
            _currentMoveDistance = 0;
        }
        else if (_sideSlipAmount > 0.5f)
        {
            DriftDistance += MoveDistance - _driftStartDistance;
            _currentMoveDistance += MoveDistance - _driftStartDistance;
            _driftStartDistance = MoveDistance;
        }
        else if (_sideSlipAmount < 0.5f && _particleShow)
        {
            _driftParticles.ForEach(p => p.Stop());
            _carMovement.Wheel.ForEach(wheel => wheel.SkidParticle.Stop());
            _particleShow = false;
            DriftTime += Time.time - _driftStartTime;
            _carMovement.StartBooster((float)_currentMoveDistance * 0.005f);
        }
    }
    
    private void MoveDistanceCheck()
    {
        float distance = Vector3.Distance(_lastPos, transform.position);
        _lastPos = transform.position;
        if (!IsInitEnd)
            return;
        MoveDistance += distance;
    }
    
    public void GameOver()
    {
        GameObject.Find("GamePause").gameObject.GetComponent<GameScreen>().OnGameEnd();
        IsInitEnd = false;
        _arrow.SetActive(false);
        Debug.Log("GameOver");

        if (_playerQuestCheck.CarController is null)
            _playerQuestCheck.CarController = this;
        
        _playerQuestCheck.AllQuestJobAct();
    }

    /// <summary>
    /// 광고본 후 그 위치 그래도 시작 
    /// </summary>
    public void GameRestart()
    {
        IsInitEnd = true;
        _arrow.SetActive(true);
    }
}

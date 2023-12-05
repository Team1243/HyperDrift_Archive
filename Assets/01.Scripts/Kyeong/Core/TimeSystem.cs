using System.Collections;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance = null; 
    
    [SerializeField] private float _startTime = 45;
    [SerializeField] private float _maxTime = 300;
    private float _curve;
    public float Curve => _curve;
    private float _currentTime = 0;
    public float CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            if (_currentTime <= 0 && !_isEnd)
            {
                _isEnd = true;
                _carController.GameOver();
            }
        }
    }

    private float _currentPlayTime = 0;
    public float CurrentPlayTime => _currentPlayTime;

    private bool _isEnd = false;
    private CarController _carController;

    private bool _gameStart = false;
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple TimeSystem is running");
        Instance = this;
        _carController = FindObjectOfType<CarController>();

        CurrentTime = _startTime;
        _curve = _maxTime;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.4f);
        _gameStart = true;
    }

    private void Update()
    {
        if (!_gameStart)
            return;
        
        CurrentTime -= Time.deltaTime;
        _currentPlayTime += Time.deltaTime;
        LifeCurve();
    }

    private void LifeCurve()
    {
        _maxTime -= Time.deltaTime;

        float time = _maxTime / 300;
        
        //easeInOutExpo
        time = time == 0 ? 0 :
            time == 1 ? 1 :
            time < 0.5f ? Mathf.Pow(2, 20 * time - 10) * .5f : (2 - Mathf.Pow(2, -20 * time + 10)) * .5f;
        
        _curve = Mathf.Lerp(0, _startTime, time);
    }

    public void AddTime()
    {
        CurrentTime = _curve;
    }
}

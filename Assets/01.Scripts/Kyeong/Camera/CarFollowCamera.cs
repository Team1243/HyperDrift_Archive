using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarFollowCamera : MonoBehaviour
{
    [Header("Core")] 
    private static CarFollowCamera _instance;

    public static CarFollowCamera Instance
    {
        get
        {
            if (_instance == null)
                UnityEngine.Debug.LogError("Instance is null");
            return _instance;
        }
    }
    
    [Header("Camera View")]
    [SerializeField] private Transform _targetTrm;
    [SerializeField] private Vector3 _lookVector = new Vector3(0, 40, -45f);
    private Vector3 _currentVector;

    [Header("Camera View Change")] 
    private Coroutine _cameraViewCoroutine = null;
    private Vector3 _startVector;
    private Vector3 _endVector;
    private float _currentViewerTime;
    private float _time;

    [Header("Camera Shake")] 
    private float _currentShakeTime;
    private Vector3 _shakePos = Vector3.zero;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        
        _currentVector = _lookVector;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (!_targetTrm)
            return; 
        
        transform.position = _targetTrm.position + _lookVector + _shakePos;
    }
    
    /// <summary>
    /// ī�޶� ���� �þư��� �������ִ� �Լ�.  
    /// </summary>
    /// <param name="percent">���� �þƿ��� �󸶳� �� ������ ������ ����</param>
    /// <param name="duration">�þư��� �ٲ�µ� �ɸ��� �ð�</param>
    /// <param name="callBack">���� ����� ���� ����� Action</param>
    public void CameraView(float percent = 100.0f, float duration = 0.5f, Action callBack = null)
    {
        if (_cameraViewCoroutine != null)
            StopCoroutine(_cameraViewCoroutine);
        _cameraViewCoroutine = StartCoroutine(CameraViewCo(percent, duration, callBack));
    }

    private IEnumerator CameraViewCo(float percent, float duration, Action callBack)
    {
        _currentViewerTime = 0;
        _startVector = _currentVector;
        _endVector = _lookVector * (percent * 0.01f);

        while (_currentViewerTime <= duration)
        {
            yield return null;
            _currentViewerTime += Time.deltaTime;
            _time = _currentViewerTime / duration;
            _currentVector = Vector3.Lerp(_startVector, _endVector, _time);
        }
        

        _currentVector = _endVector;
        callBack?.Invoke();
    }

    /// <summary>
    /// Camera Shaking ���ִ� �Լ�
    /// </summary>
    /// <param name="amplitude">�󸶳� ���ϰ� Shaking ����</param>
    /// <param name="duration">Shaking �ð�</param>
    public void CameraShake(float amplitude, float duration)
    {
        StartCoroutine(CameraShakeCo(amplitude, duration));
    } 

    private IEnumerator CameraShakeCo(float amplitude, float duration)
    {
        _currentShakeTime = 0;
        while (_currentShakeTime <= duration)
        {
            yield return null;
            _currentShakeTime += Time.deltaTime;
            _shakePos = Random.insideUnitCircle * amplitude;
        }

        _shakePos = Vector3.zero;
    }
}

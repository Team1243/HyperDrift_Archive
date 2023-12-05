using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _explosionParticle;
    private float _currentTime;

    private void Start()
    {
        //StartCoroutine(FuelRotation());
    }

    private IEnumerator FuelRotation()
    {
        _currentTime = 0;
        while (_currentTime < _duration)
        {
            yield return null;
            _currentTime += Time.deltaTime;
            float time = _currentTime / _duration;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0), time);
        }
        transform.rotation = Quaternion.Euler(0, 180, 0);
        _currentTime = 0;
        while (_currentTime < _duration)
        {
            yield return null;
            _currentTime += Time.deltaTime;
            float time = _currentTime / _duration;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 360, 0), time);
        }
        transform.rotation = Quaternion.Euler(0, 360, 0);
        StartCoroutine(FuelRotation());
    }

    public void FuelCollision()
    {
        TimeSystem.Instance.AddTime();
        GameObject particle = Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        Destroy(particle, 2f);
        Destroy(gameObject);
    }
}

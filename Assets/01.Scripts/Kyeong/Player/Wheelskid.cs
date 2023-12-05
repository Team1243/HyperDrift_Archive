using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelskid : MonoBehaviour
{
    [SerializeField] private float _intensityModifier = 1.5f;

    private Skidmarks _skidMarkController;
    private CarController _carController;
    private ParticleSystem _particleSystem;

    private int lastSkidId = -1;

    private bool _isFirst = true;
    private Vector3 _lastPos;
    
    private void Awake()
    {
        _skidMarkController = FindObjectOfType<Skidmarks>();
        _carController = transform.root.GetComponent<CarController>();
        _particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        if (_carController == null)
            return;
        
        float intensity = Mathf.Abs(_carController.SideSlipAmount);
        _particleSystem.startColor = new Color(51/255, 51/255, 51/255, intensity / 10);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarUI : MonoBehaviour
{
    private CarController _carController;
    private Image _fuelImage;

    private void Start()
    {
        _fuelImage = transform.GetChild(1).GetComponent<Image>();
        _carController = FindObjectOfType<CarController>();
    }

    private void Update()
    {
    }
}

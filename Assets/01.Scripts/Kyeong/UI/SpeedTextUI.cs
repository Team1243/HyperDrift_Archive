using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedTextUI : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void TextChange(float value)
    {
        int speed = (int)value;
        _text.text = speed.ToString(); 
    }
}

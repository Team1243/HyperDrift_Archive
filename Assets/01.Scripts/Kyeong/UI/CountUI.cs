using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CountUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _countElement;

    [SerializeField] private List<Texture2D> _countImage;
    private int _cnt = 0;

    [Header("Scale Change")] 
    [SerializeField] private float _startScale;
    [SerializeField] private float _endScale;
    [SerializeField] private float _duraction;
    private float _currentTime = 0;

    private IEnumerator Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _countElement = _uiDocument.rootVisualElement.Q<VisualElement>("Count");
        
        while (_cnt < 4)
        {
            transform.localScale = Vector3.one * _startScale;
            _countElement.style.backgroundImage = new StyleBackground(_countImage[_cnt]);

            _currentTime = 0;
            while (_currentTime < _duraction)
            {
                yield return null;
                _currentTime += Time.deltaTime;
                float time = _currentTime / _duraction;
                transform.localScale = Vector3.one * Mathf.Lerp(_startScale, _endScale, time);
            }

            transform.localScale = Vector3.one * _endScale;
            ++_cnt;
        }
        gameObject.SetActive(false);
    }
}

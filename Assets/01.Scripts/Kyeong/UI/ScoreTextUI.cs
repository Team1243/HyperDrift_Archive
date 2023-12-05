using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField] private float _changeDuration;
    private float _currentTime;
    private Coroutine _textChangeCo;
    private TextMeshProUGUI _text;
    private int _currentScore = 0;

    private void Awake()
    {
        _text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ScoreChange(int value)
    {
        if (_textChangeCo != null)
            StopCoroutine(_textChangeCo);
        _textChangeCo = StartCoroutine(ScoreTextCo(_currentScore, value));
    }

    private IEnumerator ScoreTextCo(int startScore, int endScore)
    {
        _currentTime = 0;
        while (_currentTime <= _changeDuration)
        {
            yield return null;
            _currentTime += Time.deltaTime;
            float time = _currentTime / _changeDuration;
            _currentScore = (int)Mathf.Lerp(startScore, endScore, time); 
            _text.text = _currentScore.ToString();
        }

        _currentScore = endScore;
        _text.text = _currentScore.ToString();
    }
}

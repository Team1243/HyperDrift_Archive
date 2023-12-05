using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDirectionUI : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    
    [SerializeField]private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _direction;
    private Vector2 _arrowPos;

    private void Awake()
    {
        //_uiDocument = GetComponent<UIDocument>();
        _arrowPos = new Vector2(Screen.width * .5f, Screen.height * .25f);
        _inputReader.PositionEvent += ArrowLookAt;
    }

    private void OnEnable()
    {
        _root = _uiDocument.rootVisualElement;
        _direction = _root.Q<VisualElement>("Direction");
    }

    private void OnDisable()
    {
        _inputReader.PositionEvent -= ArrowLookAt;
    }

    private void OnDestroy()
    {
        _inputReader.PositionEvent -= ArrowLookAt;
    }

    private void ArrowLookAt(Vector2 pos)
    {
        float angle = Mathf.Atan2(_arrowPos.y - pos.y, _arrowPos.x - pos.x) * Mathf.Rad2Deg * -1;
        angle += 90;
        _direction.style.rotate = new Rotate(angle);
    }
}

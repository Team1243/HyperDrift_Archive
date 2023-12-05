using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum UIState { Start, Game, Home }
[RequireComponent(typeof(UIDocument))]
public class MainUIManager : MonoBehaviour
{
    [SerializeField] protected UIDocument m_MainMenuDocument;
    public UIDocument MainMenuDocument => m_MainMenuDocument;

    [SerializeField] private MenuBar menuBar;
    //[SerializeField] private GameScreen gameScreen;

    private UIState _currentState = UIState.Home;
    public UIState currentState => _currentState;

    private Dictionary<UIState, MenuScreen> _curStateVisual = new Dictionary<UIState, MenuScreen>();
    
    void Awake()
    {
        if(m_MainMenuDocument == null)
            m_MainMenuDocument = GetComponent<UIDocument>();

        // if(gameScreen == null)
        //     gameScreen = GetScreenCode("GameScreen") as GameScreen;

        if(menuBar == null)
            menuBar = GetScreenCode("MenuBar") as MenuBar;

        _curStateVisual.Add(UIState.Home, menuBar);
        //_curStateVisual.Add(UIState.Game, gameScreen);


        foreach(var item in _curStateVisual)
            item.Value.HideScreen();

        ChangeScreen(UIState.Home);
    }

    public void ChangeScreen(UIState state)
    {
        _curStateVisual[_currentState].HideScreen();
        _currentState = state;
        _curStateVisual[_currentState].ShowScreen();
    }

    [ContextMenu("TEST")]
    public void Test()
    {
        //_curStateVisual[UIState.Home].

        ChangeScreen(UIState.Game);
    }
    //private VisualElement GetVisualElement(string name)
    //{
    //    if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
    //        return null;

    //    return GetScreenCode(name).m_s
    //}

    private MenuScreen GetScreenCode(string name)
    {
        if(string.IsNullOrEmpty(name) || m_MainMenuDocument == null) 
            return null;

        return transform.Find(name).GetComponent<MenuScreen>();
    }
}

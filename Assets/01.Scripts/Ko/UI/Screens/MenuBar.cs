using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum Buttons { Garage = 0, Main = -100, Quest = -200}
public class MenuBar : MenuScreen
{
    private bool isMoving = false;
    private homeScreen _homeScreen;
    private Buttons _currentScreen;
    private float _startTouchX;
    private float _endTouchX;
    [Tooltip("최소 움직여야 되는 거리")][SerializeField]private float _swipeDistance = 50f;

    //buttons Name
    private readonly string k_GarageScreenButton = "menu__garage-button";
    private readonly string k_HomeScreenButton = "menu__home-button";
    private readonly string k_QuestScreenButton = "menu__quest-button";
    

    //VisualElements Name
    private readonly string k_ScreenGrouup = "menu_screens-group";

    //VisualElements 
    private VisualElement m_ScreenGrouupVisualElement;

    //class
    private readonly string c_bottomButtonActive = "menu__Button--active";

    //buttons
    private Button m_GarageScreenMenuButton;
    private Button m_HomeScreenMenuButton;
    private Button m_QuestScreenMenuButton;


    private Dictionary<Buttons, Button> bottomButtons = new Dictionary<Buttons, Button>();

    private void OnEnable()
    {
        changeScreen(Buttons.Main);
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _homeScreen = GameObject.Find("homeScreen").GetComponent<homeScreen>();

        m_GarageScreenMenuButton = m_Root.Q<Button>(k_GarageScreenButton);
        m_HomeScreenMenuButton = m_Root.Q<Button>(k_HomeScreenButton);
        m_QuestScreenMenuButton = m_Root.Q<Button>(k_QuestScreenButton);

        bottomButtons.Add(Buttons.Garage, m_GarageScreenMenuButton);
        bottomButtons.Add(Buttons.Main, m_HomeScreenMenuButton);
        bottomButtons.Add(Buttons.Quest, m_QuestScreenMenuButton);


        m_ScreenGrouupVisualElement = m_Root.Q<VisualElement>(k_ScreenGrouup);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        
        m_GarageScreenMenuButton.RegisterCallback<ClickEvent>(evt => changeScreen(Buttons.Garage));
        m_HomeScreenMenuButton.RegisterCallback<ClickEvent>(evt => changeScreen(Buttons.Main));
        m_QuestScreenMenuButton.RegisterCallback<ClickEvent>(evt => changeScreen(Buttons.Quest));
        //m_HomeScreenMenuButton.RegisterCallback<ClickEvent>(S)
    }

    private void Update()
    {
        swipteInput();
    }

   

    private void changeScreen(Buttons _buutton)
    {
        if (_homeScreen.isSettingPanelShow) return;
        isMoving = true;

        Debug.Log($"현재활성화된 화면{_buutton.ToString()}");
        _currentScreen = _buutton;

        m_ScreenGrouupVisualElement.style.translate = new Translate(Length.Percent((int)_buutton), 0);

        foreach(var _dic in bottomButtons)
        {
            if(_dic.Key == _buutton)
            {
                _dic.Value.AddToClassList(c_bottomButtonActive);
                _dic.Value.RegisterCallback<TransitionEndEvent>(evt =>
                {
                    isMoving = false;
                });
            }
            else
                _dic.Value.RemoveFromClassList(c_bottomButtonActive);
        }
    }               
    
    private void swipteInput()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            _startTouchX = Input.mousePosition.x;
        else if (Input.GetMouseButtonUp(0))
        {
            _endTouchX = Input.mousePosition.x;
            UpdateSwipe();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                _startTouchX = touch.position.x;
            else if (touch.phase == TouchPhase.Ended)
            {
                _endTouchX = touch.position.x;

                UpdateSwipe();
            }
        }
#endif

    }

    private void UpdateSwipe()
    {
        if (isMoving) return;

        if (MathF.Abs(_startTouchX - _endTouchX) < _swipeDistance)
        {
            changeScreen(_currentScreen);
            return;
        }

        bool isLeft = _startTouchX < _endTouchX ? true : false;

        if (isLeft)
        {
            if (_currentScreen == Buttons.Garage) return;
                changeScreen((Buttons)((int)_currentScreen + 100));
        }
        else
        {
            if(_currentScreen == Buttons.Quest) return;
                changeScreen((Buttons)((int)_currentScreen - 100));
        }
    }
}
                        
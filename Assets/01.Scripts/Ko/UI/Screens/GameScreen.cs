using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

enum GameScreenState { Pause, End, None };
public class GameScreen : MenuScreen
{
    [SerializeField] private int _socreGoalSpeed = 2;
    private int _score = 0;
    private CarController _carController;


    //VisualElement Name
    private readonly string k_GamePause = "GamePause-VisualElement";
    private readonly string k_GameOver = "GameOver-container";
    private readonly string k_GameOverPanel = "GameOverPanel";
    private readonly string k_MoveDistanceTextContainer = "MoveDistance_text-container";
    private readonly string k_DriftDistanceTextContainer = "DriftDistance_text-container";
    private readonly string k_MoveDistanceLabelContainer = "MoveDistance-Label-Container";
    private readonly string k_LeftTimeLabelContainer = "LeftTime-Panel";

    //Button Name
    private readonly string k_ExitButton = "Exit-Button";
    private readonly string k_GameOverAdButton = "GameOver_AD-Button";
    private readonly string k_GamePauseReplayButton = "GamePause_RePlay-Button";
    private readonly string k_GamePauseHomeButton = "GamePause_Home-Button";
    private readonly string k_GamePauseButton = "GamePause-Button";

    //Label Name
    private readonly string k_ScoreLabel = "score-Label";
    private readonly string k_EndMoveDistnaceLabel = "EndMoveDistance-Label";
    private readonly string k_MoveDistanceLabel = "MoveDistance-Label";
    private readonly string k_DriftDistanceLabel = "DriftDistance-Label";
    private readonly string k_LeftTimeLabel = "LeftTime-Label";

    //Class
    private readonly string off = "off";

    //VisualElement
    private VisualElement m_GamePause;
    private VisualElement m_GameOver;
    private VisualElement m_GameOverPanel;
    private VisualElement m_MoveDistanceTextContainer;
    private VisualElement m_DriftDistanceTextContainer;
    private VisualElement m_MoveDistanceLabelContainer;
    private VisualElement m_LeftTimeLabelContainer;

    //Button
    private Button m_ExitButton;
    private Button m_GameOverAdButton;
    private Button m_GamePauseReplayButton;
    private Button m_GamePauseHomeButton;
    private Button m_GamePauseButton;

    //Label
    private Label m_ScoreLabel;
    private Label m_EndMoveDistnaceLabel;
    private Label m_DriftDistanceLabel;
    private Label m_MoveDistanceLabel;
    private Label m_LeftTimeLabel;

    [SerializeField] private UnityEngine.UI.Slider m_BoosterSlider;
    [SerializeField] private UnityEngine.UI.Image m_BoosterSliderbackGorund;
    [SerializeField] private UnityEngine.UI.Image m_BoosterSliderFill;

    private GameScreenState _state = GameScreenState.None;
    private VisualElement _root;
    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        try
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }
        catch(Exception e)
        {
            // Debug.LogException(e);  
        }

        m_GameOver = _root.Q<VisualElement>(k_GameOver);
        m_GameOverPanel = _root.Q<VisualElement>(k_GameOverPanel);
        m_MoveDistanceTextContainer = _root.Q<VisualElement>(k_MoveDistanceTextContainer);
        m_DriftDistanceTextContainer = _root.Q<VisualElement>(k_DriftDistanceTextContainer);
        m_EndMoveDistnaceLabel = _root.Q<Label>(k_EndMoveDistnaceLabel);
        m_DriftDistanceLabel = _root.Q<Label>(k_DriftDistanceLabel);

        m_GamePauseButton = _root.Q<Button>(k_GamePauseButton);
        m_GamePause = _root.Q<VisualElement>(k_GamePause);
        m_GamePauseReplayButton = _root.Q<Button>(k_GamePauseReplayButton);
        m_GamePauseHomeButton = _root.Q<Button>(k_GamePauseHomeButton);

        m_ExitButton = _root.Q<Button>(k_ExitButton);
        m_ScoreLabel = _root.Q<Label>(k_ScoreLabel);
        m_GameOverAdButton = _root.Q<Button>(k_GameOverAdButton);

        m_MoveDistanceLabel = _root.Q<Label>(k_MoveDistanceLabel);
        m_MoveDistanceLabelContainer = _root.Q<VisualElement>(k_MoveDistanceLabelContainer);
        m_LeftTimeLabel = _root.Q<Label>(k_LeftTimeLabel);
        m_LeftTimeLabelContainer = _root.Q<VisualElement>(k_LeftTimeLabel);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        Debug.Log("게임스크린 레지스터 실행됨");
        m_ExitButton.RegisterCallback<ClickEvent>(evt => { SceneManager.LoadScene("Home"); });
        //m_GameOverAdButton.RegisterCallback<ClickEvent>(evt => RewardedAdManager.Instance.ShowAd());

        m_GamePauseButton.RegisterCallback<ClickEvent>(evt =>
        {
            OnGamePause();
            Debug.Log("GamePauseActive!");
        });

        m_GamePauseReplayButton.RegisterCallback<ClickEvent>(evt => { ExitGamePuase(); });
        m_GamePauseHomeButton.RegisterCallback<ClickEvent>(evt => 
        { 
            Time.timeScale = 1;
            SceneManager.LoadScene("Home"); 
        });
    }

    private void Start()
    {
        GameObject.Find("Player").TryGetComponent<CarController>(out _carController);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _state == GameScreenState.None)
            OnGamePause();

        ChangeDistanceText();
        ChangeLeftTimeText();


        //m_BoosterSlider.value = (float)_carController.CurrentMoveDistance;
        float CurMoveDis = (float)_carController.CurrentMoveDistance;
        m_BoosterSlider.value = Mathf.Lerp(m_BoosterSlider.value, CurMoveDis, Time.deltaTime * 5);
        if(CurMoveDis <= 0.2f)
        {
            m_BoosterSliderbackGorund.color = new Color(255, 255, 255, Mathf.Lerp(m_BoosterSliderbackGorund.color.a, 0, Time.deltaTime * 2));
            m_BoosterSliderFill.color = new Color(255, 255, 255, Mathf.Lerp(m_BoosterSliderFill.color.a, 0, Time.deltaTime * 2));
        }
        else 
        {
            m_BoosterSliderbackGorund.color = new Color(255, 255, 255, Mathf.Lerp(m_BoosterSliderbackGorund.color.a, 1, Time.deltaTime * 2));
            m_BoosterSliderFill.color = new Color(255, 255, 255, Mathf.Lerp(m_BoosterSliderFill.color.a, 1, Time.deltaTime * 2));
        }
    }

    private void ChangeDistanceText()
    {
        m_MoveDistanceLabel.text = (Math.Truncate((_carController.MoveDistance / 100) * 10) / 10).ToString() + "m";
        if (m_MoveDistanceLabelContainer.resolvedStyle.width / m_MoveDistanceLabel.text.Length > m_MoveDistanceLabel.style.fontSize.value.value)
            return;
       m_MoveDistanceLabel.style.fontSize = Mathf.Clamp(new StyleLength(m_MoveDistanceLabelContainer.resolvedStyle.width / m_MoveDistanceLabel.text.Length).value.value,0, 55);
        //Debug.Log($"cur Time({Time.time})'s size : {m_MoveDistanceLabelContainer.resolvedStyle.width / m_MoveDistanceLabel.text.Length}");
    }

    private void ChangeLeftTimeText()
    {
        m_LeftTimeLabel.text = Mathf.Clamp((float)(Math.Truncate((TimeSystem.Instance.CurrentTime) * 10) / 10),0, float.MaxValue).ToString();
        if (m_LeftTimeLabelContainer.resolvedStyle.width / m_LeftTimeLabel.text.Length > m_LeftTimeLabel.style.fontSize.value.value)
            return;
        m_LeftTimeLabel.style.fontSize = new StyleLength(m_LeftTimeLabelContainer.resolvedStyle.width / m_LeftTimeLabel.text.Length);
    }

    [ContextMenu("OnGamePause")]
    public void OnGamePause()
    {
        _state = GameScreenState.Pause;
        m_GamePause.style.display = DisplayStyle.Flex;
        m_GamePause.RemoveFromClassList(off);
        Time.timeScale = 0;
    }

    [ContextMenu("OnGamePauseExit")]
    public void ExitGamePuase()
    {
        Time.timeScale = 1;
        _state = GameScreenState.None;

        m_GamePause.AddToClassList(off);
        m_GamePause.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if(_state == GameScreenState.None)
            {
                m_GamePause.style.display = DisplayStyle.None;
            }
        });
    }

    [ContextMenu("OnGameEnd")]
    public void OnGameEnd()
    {
        Debug.Log("GameEnd");
        _state = GameScreenState.End;
        //m_GameOver.style.display = DisplayStyle.Flex;
        m_GameOver.RemoveFromClassList(off);

        
        m_ScoreLabel.text = (Math.Truncate((_carController.MoveDistance / 100) * 10) / 10).ToString() + "m";
            //Debug.Log($"점수 크기{m_ScoreLabel.text.Length} ");
        //m_ScoreLabel.style.fontSize = new StyleLength(m_GameOverPanel.resolvedStyle.width / m_ScoreLabel.text.Length < 3 ? 3 : m_ScoreLabel.text.Length);
        //m_ScoreLabel.style.fontSize = new StyleLength(m_GameOverPanel.resolvedStyle.width / m_ScoreLabel.text.Length);

            m_EndMoveDistnaceLabel.text = ((int)TimeSystem.Instance.CurrentPlayTime).ToString();
            m_DriftDistanceLabel.text = ((int)_carController.DriftDistance / 100).ToString() +"m";

            int length = Mathf.Max(m_EndMoveDistnaceLabel.text.Length, m_DriftDistanceLabel.text.Length);

            //m_EndMoveDistnaceLabel.style.fontSize = new StyleLength(m_MoveDistanceTextContainer.resolvedStyle.width / length);
            m_DriftDistanceLabel.style.fontSize = new StyleLength(m_DriftDistanceTextContainer.resolvedStyle.width / length);
            
            m_ExitButton.RemoveFromClassList(off);
        
    }


    private IEnumerator ShowScore(int score, int goalTime)
    {
        float cool = goalTime / score;

        for (int i = 0; i <= score; i++)
        {
            yield return new WaitForSeconds(cool);
            m_ScoreLabel.text = i.ToString();
        }
    }

    [ContextMenu("OnGameEndExit")]
    public void OnGameEndExit()
    {
        _state = GameScreenState.None;

        m_ExitButton.AddToClassList(off);
        m_GameOver.AddToClassList(off);
    }
}

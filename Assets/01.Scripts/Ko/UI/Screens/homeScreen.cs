using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum State { On, Off };
public enum Way { left = -1, None = 0, right = +1 };

public class homeScreen : MenuScreen
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private DailyBonusPopup _dailyBonusPopup;
    [SerializeField] private HeartSystem _heartSystem; 
    public bool isSettingPanelShow {  get; private set; }

    //Map
    [SerializeField] private List<MapSO> _mapList = new List<MapSO>();
    private MapSO _curMap;
    private int _mapIndex = 0;

    //Audio Mixer
    private readonly string Mixer_Master = "MasterVolume";
    private readonly string Mixer_Music = "MusicVolume";
    private readonly string Mixer_SFX = "SFXVolume";

    //Slider Names
    private readonly string k_MasterVolumeSlider = "MasterVolume-Slider";
    private readonly string k_MusicVolumeSlider = "MusicVolume-Slider";
    private readonly string k_SFXVolumeSlider = "SFXVolume-Slider";

    //Buttons Name
    private readonly string k_StageLeftButton = "stage_left-button";
    private readonly string k_StageRightButton = "stage_right-button";
    private readonly string k_SettingPanelButton = "menu__setting-button";
    private readonly string k_SettingExitPanelButton = "Setting_exit-Button";
    private readonly string k_BattleButton = "battle-button";
    private readonly string k_DailyBonusButton = "DailyBonus-Button";
    private readonly string k_AddHeartButton = "AddHeart-Button";


    //VisualElements Name
    private readonly string k_SettingPanelVisualElement = "SettingPanel-container";
    private readonly string k_SettingPanelPopupVisualElement = "SettingPanel-VisualElement";
    private readonly string k_PopupHeartContainer = "popup_heart-container";
    private readonly string k_StageIamage = "stage-image";

    //Label Name
    private readonly string k_heartStateLabel = "heartState-label";
    private readonly string k_goldStateLabel = "goldState-label";
    private readonly string k_PopupHeartLabel = "popup_heart-label";


    //VisualElements 
    private VisualElement m_SettingPanelVisualElement;
    private VisualElement m_SettingPanelPopupVisualElement;
    private VisualElement m_PopupHeartContainer;
    private VisualElement m_StageIamage;

    //Class
    private readonly string c_off = "off";
    private readonly string c_popupDisable = "popup_panel--disable";
    private readonly string c_popupHeartShow = "on";

    //Butons
    private Button m_StageLeftButton;
    private Button m_StageRightButton;
    private Button m_SettingPanelButton;
    private Button m_SettingExitPanelButton;
    private Button m_BattleButton;
    private Button m_DailyBonusButton;
    private Button m_AddHeartButton;

    //Label
    private Label m_heartStateLabel;
    private Label m_goldStateLabel;
    private Label m_PopupHeartLabel;

    //Slider
    private Slider m_MasterVolumeSlider;
    private Slider m_MusicVolumeSlider;
    private Slider m_SFXVolumeSlider;

    private void OnEnable()
    {
        ChangeStage(Way.None);
        m_PopupHeartContainer.RemoveFromClassList(c_popupHeartShow);
        _heartSystem = FindObjectOfType<HeartSystem>();
        HeartSystem.onChangeHeartCnt += ChangeHeartLabel;
        CoinSystem.onChangeCoinCnt += ChangeCoinLabel;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        m_StageIamage = m_Root.Q<VisualElement>(k_StageIamage);
        m_StageLeftButton = m_Root.Q<Button>(k_StageLeftButton);
        m_StageRightButton = m_Root.Q<Button>(k_StageRightButton);
        m_BattleButton = m_Root.Q<Button>(k_BattleButton);


        m_SettingPanelButton = m_Root.Q<Button>(k_SettingPanelButton);
        m_SettingExitPanelButton = m_Root.Q<Button>(k_SettingExitPanelButton);
        m_SettingPanelVisualElement = m_Root.Q<VisualElement>(k_SettingPanelVisualElement);
        m_SettingPanelPopupVisualElement = m_Root.Q<VisualElement>(k_SettingPanelPopupVisualElement);
        m_MasterVolumeSlider = m_Root.Q<Slider>(k_MasterVolumeSlider);
        m_MusicVolumeSlider = m_Root.Q<Slider>(k_MusicVolumeSlider);
        m_SFXVolumeSlider = m_Root.Q<Slider>(k_SFXVolumeSlider);


        m_heartStateLabel = m_Root.Q<Label>(k_heartStateLabel);
        m_goldStateLabel = m_Root.Q<Label>(k_goldStateLabel);

        m_PopupHeartContainer = m_Root.Q<VisualElement>(k_PopupHeartContainer);
        m_PopupHeartLabel = m_Root.Q<Label>(k_PopupHeartLabel);

        m_DailyBonusButton = m_Root.Q<Button>(k_DailyBonusButton);

        m_AddHeartButton = m_Root.Q<Button>(k_AddHeartButton);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_BattleButton.RegisterCallback<ClickEvent>(GameStartButton);
        m_SettingPanelButton.RegisterCallback<ClickEvent>(showSettingPanel);
        m_SettingExitPanelButton.RegisterCallback<ClickEvent>(exitSettingPanel);

        m_StageLeftButton.RegisterCallback<ClickEvent>(evt => ChangeStage(Way.left));
        m_StageRightButton.RegisterCallback<ClickEvent>(evt => ChangeStage(Way.right));

        m_DailyBonusButton.RegisterCallback<ClickEvent>(evt => _dailyBonusPopup.ShowScreenRoutine());

        m_MasterVolumeSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            Debug.Log($"Master ner Value : {volume}");
            _audioMixer.SetFloat(Mixer_Master, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_Master, volume);
        });

        m_MusicVolumeSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            _audioMixer.SetFloat(Mixer_Music, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_Music, volume);
        });

        m_SFXVolumeSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            _audioMixer.SetFloat(Mixer_SFX, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_SFX, volume);
        });

        m_AddHeartButton.RegisterCallback<ClickEvent>(evt =>
        {
            _heartSystem.ToMaxHeart();
        });

        if (!PlayerPrefs.HasKey(Mixer_Master))
            PlayerPrefs.SetFloat(Mixer_Master, 0.5f);
        if (!PlayerPrefs.HasKey(Mixer_Music))
            PlayerPrefs.SetFloat(Mixer_Music, 0.5f);
        if (!PlayerPrefs.HasKey(Mixer_SFX))
            PlayerPrefs.SetFloat(Mixer_SFX, 0.5f);

        StartCoroutine(InitSound());
    }

    private IEnumerator InitSound()
    {
        yield return new WaitForSeconds(0.1f);
        m_MasterVolumeSlider.value = PlayerPrefs.GetFloat(Mixer_Master);
        m_MusicVolumeSlider.value = PlayerPrefs.GetFloat(Mixer_Music);
        m_SFXVolumeSlider.value = PlayerPrefs.GetFloat(Mixer_SFX);
    }

    private void ChangeStage(Way way)
    {
        _mapIndex += (int)way;
        if(_mapIndex < 0)
            _mapIndex = _mapList.Count - 1;
        else if(_mapIndex > _mapList.Count - 1)
            _mapIndex = 0;

        _curMap = _mapList[_mapIndex];
        m_StageIamage.style.backgroundImage = new StyleBackground(_curMap.mapImage);
    }

    bool isOnHearPopup = false;
    private void GameStartButton(ClickEvent evt)
    {
        
        if (!_heartSystem.IsHeartExist) 
        {
            if(!isOnHearPopup)
                StartCoroutine(ShowHeartPopup("하트가 부족합니다."));
            return;
        }
        Debug.Log("스타트!");
        
        _heartSystem.UseHeart();
        SceneManager.LoadScene(_curMap.mapSceneName);
    }

    private IEnumerator ShowHeartPopup(string text)
    {
        isOnHearPopup = true;
        m_PopupHeartContainer.AddToClassList(c_popupHeartShow);
        m_PopupHeartLabel.text = text;
        yield return new WaitForSeconds(1.2f);
        m_PopupHeartContainer.RemoveFromClassList(c_popupHeartShow);
        isOnHearPopup = false;
    }

    private void ChangeHeartLabel(int cnt)
    {
        m_heartStateLabel.text = cnt.ToString() + " / 20";    
    }

    private void ChangeCoinLabel(int cnt)
    {
        m_goldStateLabel.text = cnt.ToString();
    }


    /// <summary>
    /// 설정팝업 끄기
    /// </summary>
    /// <param name="evt"></param>
    private void exitSettingPanel(ClickEvent evt)
    {
        m_SettingPanelPopupVisualElement.AddToClassList(c_off);
        m_SettingPanelVisualElement.AddToClassList(c_off);

        isSettingPanelShow = false;
        m_SettingPanelVisualElement.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if (!isSettingPanelShow)
                m_SettingPanelVisualElement.style.display = DisplayStyle.None;
        });
    }


    /// <summary>
    /// 설정팝업 키기
    /// </summary>
    /// <param name="evt"></param>
    private void showSettingPanel(ClickEvent evt)
    {
        Debug.Log("show Setting Panel");
        isSettingPanelShow = true;
        m_SettingPanelVisualElement.style.display = DisplayStyle.Flex;
        m_SettingPanelVisualElement.RemoveFromClassList(c_off);

        m_SettingPanelPopupVisualElement.RemoveFromClassList(c_off);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DailyBonusPopup : PopupPanel
{
    [SerializeField] private RewardInfoListSO _rewardDataList;
    [SerializeField] private VisualTreeAsset _dailyRewardUI;
    [SerializeField] private DailyAttendanceSystem _dailyAttendanceSystem;

    private List<BonusUI> _bonusUiList  = new List<BonusUI>(); 

    public static Action OnUpdateInfo;
    public static Action<RewardInfo> OnRecieveReward;

    public static bool IsDayRecieve { get; set; } = false;

    //Buttons Name
    private readonly string k_PopupExitButton = "menu_popupExit-button";

    //VisualElements Name
    private readonly string k_BonusContainer = "popupDaily_bonus-container";

    //Class
    private readonly string c_popupDailyBonusOn = "on";

    //VisualElements 
    private VisualElement m_BonusContainer;

    //Buttons
    private Button m_PopupExitButton;

    private bool isShow = false;

    protected override void Awake()
    {
        base.Awake();
        //ShowScreenRoutine();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_BonusContainer = m_CurScrene.Q<VisualElement>(k_BonusContainer);
        m_PopupExitButton = m_CurScrene.Q<Button>(k_PopupExitButton);

        Debug.Log($"팝업이름 {m_CurScrene.name}");
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        m_PopupExitButton.RegisterCallback<ClickEvent>(evt => 
        {
            HideScreenRoutine(); 
        });
    }

    public override void HideScreenRoutine()
    {
        isShow = false;
        m_Popup.RemoveFromClassList(c_popupDailyBonusOn);
        m_Popup.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if (!isShow)
                SetScreenVisible(Visible.hide);
        });
    }

    public override void ShowScreenRoutine()
    {
        SetScreenVisible();
        isShow = true;
        m_Popup.AddToClassList(c_popupDailyBonusOn);

        if(_rewardDataList == null)
        {
            Debug.LogError("rewardDataList is null");
            return;
        }
        CreateDailyBonusTemplate(_rewardDataList.RewardInfoList);
    }

    public void CreateDailyBonusTemplate(List<RewardInfo> RewardInfoList)
    {
        int curDay = _dailyAttendanceSystem.NowDayPassed;
        // Debug.LogError("CreateDailyBonusTemplate: " + curDay);

        _bonusUiList.Clear();
        m_BonusContainer.Clear();
       
        foreach (var reward in RewardInfoList.Select((value, index) => (value, index)))
        {
            // Debug.Log($"리워드 생성됨 index : {reward.index}  value : {reward.value}");
            TemplateContainer template = _dailyRewardUI.Instantiate();
            m_BonusContainer.Add(template);

            BonusState bonusState = BonusState.None;
            if (reward.index == curDay && reward.value.isRecieve == false)
                bonusState = BonusState.CanRecieve;
            else if (reward.value.isRecieve)
                bonusState = BonusState.Recieved;

             BonusUI bonusUI = new BonusUI(reward.index, template, reward.value, bonusState);
             _bonusUiList.Add(bonusUI);
        }
    }
}
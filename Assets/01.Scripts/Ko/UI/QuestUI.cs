using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Recorder.OutputPath;

public class QuestUI
{
    //Label Name
    private readonly string k_QuestLabel = "Quest-Label";
    private readonly string k_QuestExplanation = "QuestExplanation-Label";
    private readonly string k_QuestReward = "QuestReward_Gold_Label";

    //Button Name
    private readonly string k_RewardButton = "QuestExplanation-Button";

    //ProgressBar Name
    private readonly string k_ProgressBar = "QuestProgress-Progress";

    //VisualElement Name
    private readonly string k_QuestClearContainer = "QuestClear-container";

    //Class
    private readonly string c_QuestClear = "QuestRunning";

    //Label
    private Label m_QuestLabel;
    private Label m_QuestDescription;
    private Label m_QuestReward;
    
    //Button
    private Button m_RewardButton;

    //ProgressBar
    private ProgressBar m_ProgressBar;

    //VisualElement
    private VisualElement m_QuestClearContainer;

    // Event
    public static event Action<RewardInfo> OnRecieveReward;

    private Quest m_Quest;
    private VisualElement m_Root;

    private bool IsRecieved = false;

    public QuestUI(Quest quest,VisualElement m_root)
    {
        m_Quest = quest;
        m_Root = m_root;

        SetVisualElements();
        RegisterButtonCallbacks();

        m_QuestLabel.text = quest.DisplayName;
        m_QuestDescription.text = quest.Description;

        UpdateRewardState();
    }

    private void SetVisualElements()
    {
        m_QuestLabel = m_Root.Q<Label>(k_QuestLabel);
        m_QuestDescription = m_Root.Q<Label>(k_QuestExplanation);
        m_QuestReward = m_Root.Q<Label>(k_QuestReward);

        m_RewardButton = m_Root.Q<Button>(k_RewardButton);
        m_ProgressBar = m_Root.Q<ProgressBar>(k_ProgressBar);
        m_QuestClearContainer = m_Root.Q<VisualElement>(k_QuestClearContainer);
    }

    private void RegisterButtonCallbacks()
    {
        m_RewardButton.RegisterCallback<ClickEvent>(OnRewardReceiveButton);
    }

    private void OnRewardReceiveButton(ClickEvent evt)
    {
        if (!m_Quest.IsComplete)
            return;
        Debug.Log("Button Pressed");

        //m_QuestClearContainer.AddToClassList(c_QuestClear);

        IsRecieved = true;
        m_QuestLabel.text = "퀘스트 완료!";
        m_QuestDescription.text = "퀘스트를 완료하였습니다!";
        m_QuestReward.text = "0";

        OnRecieveReward?.Invoke(m_Quest.RewardInfo);
        m_Quest.State = QuestState.Deactivate;

        m_QuestClearContainer.RemoveFromClassList(c_QuestClear);
    }

    /// <summary>
    /// QuestUI현제 상태 업데이트
    /// </summary>
    /// <param name="evt"></param>
    public void UpdateRewardState()
    {
        m_QuestReward.text = m_Quest.RewardInfo.RewardAmount.ToString();

        m_ProgressBar.highValue = m_Quest.GetCompleteProgressValue;
        m_ProgressBar.value = m_Quest.GetCurrentProgress;
        m_ProgressBar.title = $"{m_Quest.GetCurrentProgress} / {m_Quest.GetCompleteProgressValue}";

        Debug.Log($"{m_Quest.DisplayName}'s state : " + m_Quest.State);

        if (m_Quest.IsDeactivate)
            m_QuestClearContainer.RemoveFromClassList(c_QuestClear);

        if (m_Quest.IsComplete)
            m_RewardButton.RemoveFromClassList("off");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestScreen : MenuScreen
{
    [SerializeField] private VisualTreeAsset _questVisual;
    //[SerializeField] private List<Quest> _quests = new List<Quest>();
    [SerializeField] private QuestInfoListSO _questInfoList;
    [SerializeField] private List<QuestUI> _questUIs = new List<QuestUI>();

    //VisualElement Name
    private readonly string k_questListVisualElement = "Quest-List";

    //Label Name
    private readonly string k_heartStateLabel = "Quest_heartState-label";
    private readonly string k_goldStateLabel = "Quest_goldState-label";

    //Calss Name
    private readonly string c_quest = "QuestContainer";
    //Label
    private Label m_heartStateLabel;
    private Label m_goldStateLabel;

    //VisualElement
    private VisualElement m_questListVisualElement;

    private void OnEnable()
    {
        HeartSystem.onChangeHeartCnt += Quest_ChangeHeartLabel;
        CoinSystem.onChangeCoinCnt += Quest_ChangeCoinLabel;
        //UpdateeQuestUI();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_heartStateLabel = m_Root.Q<Label>(k_heartStateLabel);
        m_goldStateLabel = m_Root.Q<Label>(k_goldStateLabel);

        m_questListVisualElement = m_Root.Q<VisualElement>(k_questListVisualElement);
    }

    public void UpdateeQuestUI()
    {
        _questUIs.Clear();
        m_questListVisualElement.Clear();

        foreach (var quest in _questInfoList.QuestList)
        {
            if (quest.IsRecieveReward)
                return;
            TemplateContainer template = _questVisual.Instantiate();
            template.AddToClassList(c_quest);
            m_questListVisualElement.Add(template);

            QuestUI questUI = new QuestUI(quest, template);
            _questUIs.Add(questUI);
        }
    }

    private void Quest_ChangeHeartLabel(int cnt)
    {
        m_heartStateLabel.text = cnt.ToString() + " / 20";
    }

    private void Quest_ChangeCoinLabel(int cnt)
    {
        m_goldStateLabel.text = cnt.ToString();
    }
}

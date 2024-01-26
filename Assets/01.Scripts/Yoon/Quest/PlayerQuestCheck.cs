using UnityEngine;

public class PlayerQuestCheck : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private CarController _carController;
    public CarController CarController
    {
        get => _carController;
        set => _carController = value;
    }
    
    [Header("DriftTimeQuest")]
    [SerializeField] private Quest driftTimeQuest;
    [SerializeField] private float driftTimeGoalValue;
    [SerializeField] private int driftTimeRewardAmount;

    [Header("MoveDistanceQuest")]
    [SerializeField] private Quest moveDistanceQuest;
    [SerializeField] private float moveDistanceGoalValue;
    [SerializeField] private int moveDistanceRewardAmount;

    [Header("HighSpeedQuest")]
    [SerializeField] private Quest highSpeedQuest;
    [SerializeField] private float highSpeedGoalValue;
    [SerializeField] private int highSpeedRewardAmount;

    private void Start()
    {
        driftTimeQuest.OnRunning();
        RewardInfo rewardInfoA = new RewardInfo(RewardType.Coin, driftTimeRewardAmount);
        driftTimeQuest.SetRewardInfo(rewardInfoA);

        moveDistanceQuest.OnRunning();
        RewardInfo rewardInfoB = new RewardInfo(RewardType.Coin, moveDistanceRewardAmount);
        moveDistanceQuest.SetRewardInfo(rewardInfoB);

        highSpeedQuest.OnRunning();
        RewardInfo rewardInfoC = new RewardInfo(RewardType.Coin, highSpeedRewardAmount);
        highSpeedQuest.SetRewardInfo(rewardInfoC);

        FindObjectOfType<QuestScreen>().UpdateQuestUI();
    }

    // GameOver�� �� �ش� �Լ� ����
    [ContextMenu("Check")]
    public void AllQuestJobAct()
    {
        driftTimeQuest.JobAct(DriftTimeCondition());
        moveDistanceQuest.JobAct(MoveDistanceCondition());
        highSpeedQuest.JobAct(HighSpeedCondition());
    }

    // DailyQuestSystem���� ����Ʈ �ʱ�ȭ
    [ContextMenu("Init")]
    public void AllQuestJobInit()
    {
        driftTimeQuest.Reset();
        moveDistanceQuest.Reset();
        highSpeedQuest.Reset();

        FindObjectOfType<QuestScreen>().UpdateQuestUI();
    }

    #region Conditions

    public bool DriftTimeCondition()
    {
        if (_carController.DriftTime >= driftTimeGoalValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MoveDistanceCondition()
    {
        if (_carController.MoveDistance >= moveDistanceGoalValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HighSpeedCondition()
    {
        if (_carController.HighSpeed >= highSpeedGoalValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}

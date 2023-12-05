using Unity.VisualScripting.Dependencies.NCalc;
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

    private void Awake()
    {
        // _carController = player.GetComponent<CarController>();
        _carController = FindObjectOfType<CarController>();
    }

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
    }

    // GameOver일 때 해당 함수 실행
    [ContextMenu("Check")]
    public void AllQuestJobAct()
    {
        driftTimeQuest.JobAct(DriftTimeCondition());
        moveDistanceQuest.JobAct(MoveDistanceCondition());
        highSpeedQuest.JobAct(HighSpeedCondition());
    }

    // DailyQuestSystem에서 퀘스트 초기화
    [ContextMenu("Init")]
    public void AllQuestJobInit()
    {
        driftTimeQuest.JobInit();
        moveDistanceQuest.JobInit();
        highSpeedQuest.JobInit();
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

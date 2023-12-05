using UnityEngine;

public enum QuestState
{
    // 순서대로 등록만, 진행 중, 완료
    Registered,
    Running,    
    Complete,
}

[CreateAssetMenu(fileName = "QeustSO", menuName = "SO/Quest/QeustSO")]
public class Quest : ScriptableObject
{
    #region Property

    // Qeust 구분을 위한 ID
    [Header("Id")]
    [SerializeField] private string id;
    public string Id
    {
        get => id;
        set => id = value;
    }

    // Quest 정보 표시
    #region Quest Info

    [Header("QeustInfo")]
    [SerializeField] [TextArea(0, 5)] private string displayName;
    [SerializeField] [TextArea(3, 5)] private string description;
    public string DisplayName
    {
        get => displayName;
        set => displayName = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }

    #endregion

    // Qeust 작업
    #region Quest Job

    // Qeust가 해주는 작업
    [Header("JobInfo")]
    [SerializeField] private Job job;
    public Job Job
    {
        get => job;
        set => job = value;
    }
    public int GetCurrentProgress => job.CurrentProgressValue;
    public int GetCompleteProgressValue => job.GoalProgressValue;

    #endregion

    // Quest 상태
    #region Quest State

    private QuestState state = QuestState.Registered;
    public QuestState State
    {
        get => state;
        set => state = value;
    }
    
    public bool IsRegistered => state == QuestState.Registered;
    public bool IsRunning => state == QuestState.Running;
    public bool IsComplete => state == QuestState.Complete;

    #endregion

    // Qeust 이벤트, 리워드 정보
    #region Quest Event and Reward

    // Quest 상태에 따른 이벤트
    public delegate void OnQuestCompleted();
    public delegate void OnChangedProgress();
    public event OnQuestCompleted onQeustCompleted;
    public event OnChangedProgress onChangedProgress;

    // Quest 보상 정보
    private RewardInfo rewardInfo;
    public void SetRewardInfo(RewardInfo info) => rewardInfo = info;
    public RewardInfo GetRewardInfo() => rewardInfo;
    public bool IsRecieveReward => rewardInfo.isRecieve;

    #endregion

    #endregion

    #region Method

    // ----------------<멤버 함수들>----------------

    // 등록하고 나서 퀘스트를 활성화하는 함수
    public void OnRunning()
    {
        state = QuestState.Running;
        job.Setup(this);

        onChangedProgress?.Invoke(); // UI Update

        Debug.Log("Quest Running !!!");
    }

    // 퀘스트의 완료조건을 충족하였을 때 실행시켜주는 함수
    public void OnComplete()
    {
        state = QuestState.Complete;
        job.OnComplete();
        
        onQeustCompleted?.Invoke();  // complete event
        // onChangedProgress?.Invoke(); // UI Update

        onQeustCompleted = null;
        onChangedProgress = null;

        Debug.Log("Quest Complete !!!");
    }

    public void RecieveReport(int value)
    {
        onChangedProgress?.Invoke(); // UI Update
    }

    // 게임이 끝날 때마다 실행
    public void JobAct(bool condition)
    {
        job.SendReport(condition);
    }

    // 작업 초기화 (퀘스트가 완료되고 보상까지 받았을 때 실행)
    public void JobInit()
    {
        job.Init();
    }

    #endregion

}
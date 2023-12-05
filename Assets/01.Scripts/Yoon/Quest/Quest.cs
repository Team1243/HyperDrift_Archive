using UnityEngine;

public enum QuestState
{
    // ������� ��ϸ�, ���� ��, �Ϸ�
    Registered,
    Running,    
    Complete,
}

[CreateAssetMenu(fileName = "QeustSO", menuName = "SO/Quest/QeustSO")]
public class Quest : ScriptableObject
{
    #region Property

    // Qeust ������ ���� ID
    [Header("Id")]
    [SerializeField] private string id;
    public string Id
    {
        get => id;
        set => id = value;
    }

    // Quest ���� ǥ��
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

    // Qeust �۾�
    #region Quest Job

    // Qeust�� ���ִ� �۾�
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

    // Quest ����
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

    // Qeust �̺�Ʈ, ������ ����
    #region Quest Event and Reward

    // Quest ���¿� ���� �̺�Ʈ
    public delegate void OnQuestCompleted();
    public delegate void OnChangedProgress();
    public event OnQuestCompleted onQeustCompleted;
    public event OnChangedProgress onChangedProgress;

    // Quest ���� ����
    private RewardInfo rewardInfo;
    public void SetRewardInfo(RewardInfo info) => rewardInfo = info;
    public RewardInfo GetRewardInfo() => rewardInfo;
    public bool IsRecieveReward => rewardInfo.isRecieve;

    #endregion

    #endregion

    #region Method

    // ----------------<��� �Լ���>----------------

    // ����ϰ� ���� ����Ʈ�� Ȱ��ȭ�ϴ� �Լ�
    public void OnRunning()
    {
        state = QuestState.Running;
        job.Setup(this);

        onChangedProgress?.Invoke(); // UI Update

        Debug.Log("Quest Running !!!");
    }

    // ����Ʈ�� �Ϸ������� �����Ͽ��� �� ��������ִ� �Լ�
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

    // ������ ���� ������ ����
    public void JobAct(bool condition)
    {
        job.SendReport(condition);
    }

    // �۾� �ʱ�ȭ (����Ʈ�� �Ϸ�ǰ� ������� �޾��� �� ����)
    public void JobInit()
    {
        job.Init();
    }

    #endregion

}
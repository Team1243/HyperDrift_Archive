using UnityEngine;

[CreateAssetMenu(fileName = "JobSO", menuName = "SO/Quest/JobSO")]
public class Job : ScriptableObject
{
    #region Property

    // �� �۾��� �� �ִ� ����Ʈ(����, �θ� ����)
    public Quest Owner { get; private set; }

    // ���� (UI ǥ������ ���� ����)
    [SerializeField] [TextArea(0, 5)] private string displayName;
    [SerializeField] [TextArea(3, 5)] private string description;

    // �׼� (�۾� ����)
    [SerializeField] private JobAction action;

    // Ÿ�� (�۾��� ���) �������� �ʿ����� ����. ���߿� �ʿ��ϸ� �����ϵ��� ����.

    // ���൵
    [SerializeField] private int goalProgressValue;
    public int GoalProgressValue
    {
        get => goalProgressValue;
        set => goalProgressValue = value;
    }
    private int currentProgressValue = 0;
    public int CurrentProgressValue
    {
        get => currentProgressValue;
        set
        {
            if (isComplete) return;

            int prevSuccess = currentProgressValue;
            currentProgressValue = value;
            // currentProgressValue = Mathf.Clamp(value, 0, goalProgressValue);

            // ���� �����ġ�� ��ǥ�� �ϴ� ��ġ�� �Ѿ��� ��
            if (goalProgressValue <= currentProgressValue)
            {
                // job complete event
                Debug.Log("Job Complete");
                onJobCompleted();
            }
        }
    }

    private string saveKey = "CurrentProgressValue";

    // �̺�Ʈ
    public delegate void OnJobCompleted();
    public OnJobCompleted onJobCompleted;

    private bool isComplete = false;

    #endregion

    // �θ� ����Ʈ ����
    public void Setup(Quest owner)
    {
        Owner = owner;
        onJobCompleted = owner.OnComplete;
    }

    // ��� Qeust���� ���൵�� �����ִ� ���� (������ ���� ������)
    public void SendReport(bool condition)
    {
        if (isComplete) return;

        CurrentProgressValue = action.Run(condition, CurrentProgressValue);
        // Debug.Log("update: " + currentProgressValue);
        Owner.RecieveReport(CurrentProgressValue);
    }

    // �Ϸ����� ��
    public void OnComplete()
    {
        currentProgressValue = goalProgressValue;
        isComplete = true;
    }

    // ��Ȱ��ȭ
    public void Init()
    {
        Owner = null;
        currentProgressValue = 0;
        onJobCompleted = null;
        isComplete = false;
        PlayerPrefs.DeleteKey(saveKey);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            currentProgressValue = PlayerPrefs.GetInt(saveKey);
            // Debug.Log("load: " + currentProgressValue);
        }
        else
        {
            // Debug.Log("save: " + currentProgressValue);
            PlayerPrefs.SetInt(saveKey, currentProgressValue);
            PlayerPrefs.Save();
        }
    }

    private void OnDisable()
    {
        // Debug.Log("save: " + currentProgressValue);
        PlayerPrefs.SetInt(saveKey, currentProgressValue);
        PlayerPrefs.Save();
    }

}
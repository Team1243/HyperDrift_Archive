using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
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
    // [SerializeField] private JobAction action;

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

    [SerializeField] private string saveKey = "CurrentProgressValue";

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

        if (Owner.IsRunning)
        {
            CurrentProgressValue = Run(condition, CurrentProgressValue);
            Debug.Log("update: " + currentProgressValue);
            Owner.RecieveReport(CurrentProgressValue);
        }
    }

    public int Run(bool condition, int currentProgress)
    {
        if (condition)
        {
            return ++currentProgress;
        }
        else
        {
            return currentProgress;
        }
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
        SceneManager.sceneLoaded += OnSceneLoaded;

        Load();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Save();
        Load();
    }

    private void OnDisable()
    {
        Save();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Save()
    {
        // Debug.Log("save: " + currentProgressValue);
        PlayerPrefs.SetInt(saveKey, currentProgressValue);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            currentProgressValue = PlayerPrefs.GetInt(saveKey);
            // Debug.Log("load: " + currentProgressValue);
        }
        else
        {
            currentProgressValue = 0;
            PlayerPrefs.SetInt(saveKey, currentProgressValue);
            PlayerPrefs.Save();
        }
    }

}
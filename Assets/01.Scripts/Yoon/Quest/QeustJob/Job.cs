using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
[CreateAssetMenu(fileName = "JobSO", menuName = "SO/Quest/JobSO")]
public class Job : ScriptableObject
{
    #region Property

    // 이 작업이 들어가 있는 퀘스트(주인, 부모 느낌)
    public Quest Owner { get; private set; }

    // 정보 (UI 표시하지 않을 거임)
    [SerializeField] [TextArea(0, 5)] private string displayName;
    [SerializeField] [TextArea(3, 5)] private string description;

    // 액션 (작업 로직)
    // [SerializeField] private JobAction action;

    // 타겟 (작업의 대상) 아직까지 필요하지 않음. 나중에 필요하면 구현하도록 하자.

    // 진행도
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

            // 현재 진행수치가 목표로 하는 수치를 넘었을 때
            if (goalProgressValue <= currentProgressValue)
            {
                // job complete event
                Debug.Log("Job Complete");
                onJobCompleted();
            }
        }
    }

    [SerializeField] private string saveKey = "CurrentProgressValue";

    // 이벤트
    public delegate void OnJobCompleted();
    public OnJobCompleted onJobCompleted;

    private bool isComplete = false;

    #endregion

    // 부모 퀘스트 세팅
    public void Setup(Quest owner)
    {
        Owner = owner;
        onJobCompleted = owner.OnComplete;
    }

    // 계속 Qeust에게 진행도를 보내주는 거임 (게임이 끝날 때마다)
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

    // 완료했을 때
    public void OnComplete()
    {
        currentProgressValue = goalProgressValue;
        isComplete = true;
    }

    // 비활성화
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
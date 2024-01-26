using UnityEngine.SceneManagement;
using UnityEngine;

public class DailyAttendanceSystem : MonoBehaviour
{
    public RewardInfoListSO dailyRewardInfoListSO;

    private int nowDayPassed = 0;
    public int NowDayPassed => nowDayPassed;
    private string saveKey = "NowDayPassed";

    private bool IsAWeekDone()
    {
        foreach (var d  in dailyRewardInfoListSO.RewardInfoList)
        {
            if (false == d.isRecieve)
            {
                return false;
            }
        }
        return true;
    }

    private void Awake()
    {
        // �ʱ�ȭ
        // PlayerPrefs.DeleteAll();
        // InitRewardDataAWeek();

        LoadDayData();
        
        RealDailyTimeSystem.OnDayHasPassed += UpdateDailyReward;
        DailyBonusPopup.OnUpdateInfo += OnRecieveReward;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveDayData();
        LoadDayData();
    }

    private void OnDisable()
    {
        SaveDayData();
    }

    public void SaveDayData()
    {
        PlayerPrefs.SetInt(saveKey, nowDayPassed);
        PlayerPrefs.Save();
    }

    public void LoadDayData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            nowDayPassed = PlayerPrefs.GetInt(saveKey);
        }
        else
        {
            SaveDayData();
        }
    }

    private void DayRecieveRewardCheck(int day)
    {
        dailyRewardInfoListSO.RewardInfoList[day].isRecieve = true;
    }

    private bool IsDayRecieveReward(int day)
    {
        return dailyRewardInfoListSO.RewardInfoList[day].isRecieve;
    }

    private void OnRecieveReward()
    {
        DayRecieveRewardCheck(nowDayPassed);
    }

    private void UpdateDailyReward()
    {
        // 7�������� ��� ������ �޾Ҵٸ�
        if (IsAWeekDone())
        {
            InitRewardDataAWeek();
            return;
        }

        if (nowDayPassed < 6 && IsDayRecieveReward(nowDayPassed))
        {
            nowDayPassed++;
            Debug.Log("UpdateDailyReward NowDayPassed : " + NowDayPassed);
        }
    }

    private void InitRewardDataAWeek()
    {
        dailyRewardInfoListSO.ResetInfoData();
        nowDayPassed = 0;
        PlayerPrefs.DeleteKey(saveKey);
        SaveDayData();
    }
}

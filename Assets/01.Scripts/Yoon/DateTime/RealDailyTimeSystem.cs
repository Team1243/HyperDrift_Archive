using System.Collections;
using UnityEngine;
using System;

public class RealDailyTimeSystem : MonoBehaviour
{
    private DateTime lastDateTime;
    private string lastDateTimeTxtData;
    private string saveKey = "LastDateTime";
   
    private TimeSpan passedTime;
    private int closeToNextDate = 23;

    // 출석부, 일일퀘스트 시스템이랑 연결되어 있을 예정
    public static event Action OnDayHasPassed; 

    [SerializeField] private float checkDelayTime = 60f;

    private void Start()
    {
        // 데이터 초기화
        // PlayerPrefs.DeleteKey(saveKey);
        // LoadLastData();
        // SaveNowData();

        // lastDayTime 데이터 가져오기
        LoadLastData();

        // 최소 하루가 지났다면
        if (IsNewDay())
        {
            SaveNowData();
            OnDayHasPassed?.Invoke();
        }
        else
        {
            // 경과 시간 구하기
            GetPassedTimeUntillNow(lastDateTime, out passedTime);

            // 마지막 하루가 지난 시점부터 23시간이 지났다면 주기적으로 12시가 지났는지 확인해주어야 함
            if (passedTime >= TimeSpan.FromHours(closeToNextDate))
            {
                StartCoroutine(DateCheckLoop());
            }
        }
    }

    private IEnumerator DateCheckLoop()
    {
        while (true)
        {
            if (IsNewDay())
            {
                SaveNowData();
                OnDayHasPassed?.Invoke(); 
                yield break;
            }

            yield return new WaitForSeconds(checkDelayTime);
        }
    }

    // for debugging
    private void DateDebug()
    {
        Debug.Log($"<Data> LastTime: {lastDateTime}");
    }

    #region LoadAndSave

    private void LoadLastData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            lastDateTimeTxtData = PlayerPrefs.GetString(saveKey);
            lastDateTime = DateTime.Parse(lastDateTimeTxtData);
        }
        else
        {
            SaveNowData();
        }
    }

    private void SaveNowData()
    {
        NowTimeDataSet();

        PlayerPrefs.SetString(saveKey, lastDateTimeTxtData);
        PlayerPrefs.Save();
    }

    private void NowTimeDataSet()
    {
        lastDateTime = DateTime.Now;
        lastDateTimeTxtData = lastDateTime.ToString();
    }

    #endregion

    #region Calculate

    private void GetPassedTimeUntillNow(DateTime lastTime, out TimeSpan passedTime)
    {
        passedTime = DateTime.Now - lastTime;
    }

    private bool IsNewDay()
    {
        // 경과 시간 구하기
        GetPassedTimeUntillNow(lastDateTime, out passedTime);

        if (passedTime >= TimeSpan.FromHours(24))
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

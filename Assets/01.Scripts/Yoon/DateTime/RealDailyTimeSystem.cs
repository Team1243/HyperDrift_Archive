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

    // �⼮��, ��������Ʈ �ý����̶� ����Ǿ� ���� ����
    public static event Action OnDayHasPassed; 

    [SerializeField] private float checkDelayTime = 60f;

    private void OnEnable()
    {
        // ������ �ʱ�ȭ
        // PlayerPrefs.DeleteAll();
        // LoadLastData();
        // SaveNowData();

        // lastDayTime ������ ��������
        LoadLastData();

        // �ּ� �Ϸ簡 �����ٸ�
        if (IsNewDay())
        {
            Debug.Log("passed");
            DateDebug();

            SaveNowData();
            OnDayHasPassed?.Invoke();
        }
        else
        {
            Debug.Log("not passed");
            DateDebug();

            // ��� �ð� ���ϱ�
            GetPassedTimeUntillNow(lastDateTime, out passedTime);

            // ������ �Ϸ簡 ���� �������� 23�ð��� �����ٸ� �ֱ������� 12�ð� �������� Ȯ�����־�� ��
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
            Debug.Log("have not savekey");
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
        // ��� �ð� ���ϱ�
        // GetPassedTimeUntillNow(lastDateTime, out passedTime);
        // Debug.Log(passedTime);
        // 
        // if (passedTime >= TimeSpan.FromMinutes(2))
        // {
        //     return true;
        //     // StartCoroutine(DateCheckLoop());
        // }
        // else
        // {
        //     return false;
        // }

        return DateTime.Now.Date > lastDateTime.Date;
    }

    #endregion
}

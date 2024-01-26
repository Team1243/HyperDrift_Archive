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

    private void Start()
    {
        // ������ �ʱ�ȭ
        // PlayerPrefs.DeleteKey(saveKey);
        // LoadLastData();
        // SaveNowData();

        // lastDayTime ������ ��������
        LoadLastData();

        // �ּ� �Ϸ簡 �����ٸ�
        if (IsNewDay())
        {
            SaveNowData();
            OnDayHasPassed?.Invoke();
        }
        else
        {
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

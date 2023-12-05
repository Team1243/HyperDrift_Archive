using System.Collections;
using UnityEngine;
using System;

public class HeartSystem : MonoBehaviour
{
    private HeartSystemData heartSystemData;

    public bool IsHeartExist => heartSystemData.IsHeartExist;

    [SerializeField] private float checkDelayTime = 60f;

    private Coroutine heartGenerateCor = null;

    private string saveKey = "HeartSystemData";

    public static Action<int> onChangeHeartCnt; // ui update

    private void Start()
    {
        // PlayerPrefs.DeleteAll();

        heartSystemData = new HeartSystemData();
        LoadLastData();

        onChangeHeartCnt?.Invoke(heartSystemData.CurrentHeart);

        if (!heartSystemData.IsHeartMax)
        {
            ReActivateHeartCor(heartGenerateCor);
        }
    }

    // ��Ʈ ���� ����
    private IEnumerator HeartGeneratingLoop()
    {
        TimeSpan passedTime;
        while (true)
        {
            passedTime = DateTime.Now - heartSystemData.LastGenerateTime;

            if (passedTime >= TimeSpan.FromMinutes(heartSystemData.GenerateDelayTime))
            {
                GenerateHeart();
            }

            yield return new WaitForSeconds(checkDelayTime);
        }
    }

    #region LoadAndSave

    // ���� �ֱ� ���� �������� ���� �����͸� ������
    private void LoadLastData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            heartSystemData = JsonUtility.FromJson<HeartSystemData>(json);
            heartSystemData.LastGenerateTime = DateTime.Parse(heartSystemData.LastGenerateTimeTxtData);

            TimeSpan passedTime = DateTime.Now - heartSystemData.LastGenerateTime;
            if (passedTime.TotalMinutes >= heartSystemData.GenerateDelayTime)
            {
                var plusAmt = passedTime.TotalMinutes / heartSystemData.GenerateDelayTime;
                GenerateHeart((int)plusAmt);
                // ReActivateHeartCor(heartGenerateCor);
            }
        }
        else
        {
            heartSystemData.CurrentHeart = heartSystemData.MaxHeart;
            heartSystemData.LastGenerateTime = DateTime.Now;
            heartSystemData.LastGenerateTimeTxtData = heartSystemData.LastGenerateTime.ToString();
        }
    }

    // ���� �����͸� ������
    private void SaveNowData()
    {
        // heartSystemData.DataDebug();
        string json = JsonUtility.ToJson(heartSystemData);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    #endregion

    #region Heart

    // ��Ʈ ���
    [ContextMenu("UseHaert")]
    public void UseHeart()
    {
        // ��Ʈ�� ������
        if (heartSystemData.IsHeartExist)
        {
            heartSystemData.CurrentHeart--;

            SaveNowData();
            onChangeHeartCnt?.Invoke(heartSystemData.CurrentHeart);
        }
        // ��Ʈ�� 0���� ��
        else
        {
            Debug.Log("Heart is not exist");
        }

        if (!heartSystemData.IsHeartMax)
        {
            ReActivateHeartCor(heartGenerateCor);
        }
    }

    // ��Ʈ ����
    public void GenerateHeart(int plusAmount = 1, bool isReward = false)
    {
        // ������ �ƴ϶� �Ϲ����� ��Ʈ �����̸�
        if (!isReward)
        {
            // ��Ʈ�� �����ϴµ� �ִ� ������ ������ �ʾ��� ��
            if (heartSystemData.IsHeartExist && !heartSystemData.IsHeartMax)
            {
                heartSystemData.CurrentHeart = Mathf.Min(heartSystemData.MaxHeart, heartSystemData.CurrentHeart + plusAmount);
                heartSystemData.LastGenerateTime = DateTime.Now;
                heartSystemData.LastGenerateTimeTxtData = heartSystemData.LastGenerateTime.ToString();
            }
        }
        // �����̸�
        else
        {
            heartSystemData.CurrentHeart += plusAmount;
        }

        SaveNowData();
        onChangeHeartCnt?.Invoke(heartSystemData.CurrentHeart); 

        if (heartSystemData.IsHeartMax)
        {
            Debug.Log("Heart is max");

            if (heartGenerateCor != null)
            {
                StopCoroutine(heartGenerateCor);
            }
        }
    }

    #endregion

    #region ETC

    private void ReActivateHeartCor(Coroutine cor)
    {
        if (cor == null)
        {
            heartSystemData.LastGenerateTime = DateTime.Now;
            FindObjectOfType<PlayerQuestCheck>().CarController = FindObjectOfType<CarController>();
            heartGenerateCor = StartCoroutine(HeartGeneratingLoop());
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveNowData();
    }

    #endregion

    [ContextMenu("ToMaxHeart")]
    public void ToMaxHeart()
    {
        heartSystemData.CurrentHeart += heartSystemData.MaxHeart - heartSystemData.CurrentHeart;
    }
}

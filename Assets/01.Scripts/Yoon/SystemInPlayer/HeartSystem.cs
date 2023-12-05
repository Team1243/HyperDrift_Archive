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

    // 하트 생성 루프
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

    // 가장 최근 게임 종료했을 때의 데이터를 가져옴
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

    // 현재 데이터를 저장함
    private void SaveNowData()
    {
        // heartSystemData.DataDebug();
        string json = JsonUtility.ToJson(heartSystemData);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    #endregion

    #region Heart

    // 하트 사용
    [ContextMenu("UseHaert")]
    public void UseHeart()
    {
        // 하트가 있으면
        if (heartSystemData.IsHeartExist)
        {
            heartSystemData.CurrentHeart--;

            SaveNowData();
            onChangeHeartCnt?.Invoke(heartSystemData.CurrentHeart);
        }
        // 하트가 0개일 때
        else
        {
            Debug.Log("Heart is not exist");
        }

        if (!heartSystemData.IsHeartMax)
        {
            ReActivateHeartCor(heartGenerateCor);
        }
    }

    // 하트 생성
    public void GenerateHeart(int plusAmount = 1, bool isReward = false)
    {
        // 보상이 아니라 일반적인 하트 생성이면
        if (!isReward)
        {
            // 하트가 존재하는데 최대 개수를 넘지는 않았을 때
            if (heartSystemData.IsHeartExist && !heartSystemData.IsHeartMax)
            {
                heartSystemData.CurrentHeart = Mathf.Min(heartSystemData.MaxHeart, heartSystemData.CurrentHeart + plusAmount);
                heartSystemData.LastGenerateTime = DateTime.Now;
                heartSystemData.LastGenerateTimeTxtData = heartSystemData.LastGenerateTime.ToString();
            }
        }
        // 보상이면
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

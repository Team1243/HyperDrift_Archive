using UnityEngine;
using System;

public class CoinSystem : MonoBehaviour
{
    // 가지고 있는 코인의 수
    private int coinCount;
    private string saveKey = "CoinCount";

    private int defaultCoinCount = 100;
    private int maxCoinCount = 999999;

    public static Action<int> onChangeCoinCnt;

    public bool IsCoinEnough(int compareTargetCnt) => (coinCount >= compareTargetCnt);

    private void Start()
    {
        // PlayerPrefs.DeleteAll();

        LoadCoinData();
        onChangeCoinCnt?.Invoke(coinCount);
    }

    #region LoadAndSave

    private void LoadCoinData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            coinCount = PlayerPrefs.GetInt(saveKey);
            // Debug.Log("<LoadData> " + coinCount);
        }
        else
        {
            coinCount = defaultCoinCount;
            Debug.Log("<LoadData> " + coinCount);
        }
    }

    private void SaveCoinData()
    {
        PlayerPrefs.SetInt(saveKey, coinCount);
        PlayerPrefs.Save();
        // Debug.Log("<SaveData> " + coinCount);
    }

    #endregion

    #region Coin

    [ContextMenu("Test")]
    public void Test()
    {
        UseCoin(10);
    }

    // 코인을 사용하는 함수
    public void UseCoin(int useCoinCnt)
    {
        // 해당 함수를 사용하기 전에 필요한 코인의 양이 충분한지 확인하고 이 함수를 실행해주는 것
        coinCount = Mathf.Clamp(coinCount - useCoinCnt, 0, maxCoinCount);

        // 데이터 저장하고, 
        SaveCoinData();
        // UI Update 하기
        onChangeCoinCnt?.Invoke(coinCount);
    }

    // 코인을 받는 함수
    public void RecieveCoin(int reciveCoinCnt)
    {
        coinCount = Mathf.Clamp(coinCount + reciveCoinCnt, 0, maxCoinCount);

        // 데이터 저장하고, 
        SaveCoinData();
        // UI Update 하기
        onChangeCoinCnt?.Invoke(coinCount);
    }

    #endregion
}

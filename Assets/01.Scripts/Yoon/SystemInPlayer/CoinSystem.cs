using UnityEngine;
using System;

public class CoinSystem : MonoBehaviour
{
    // ������ �ִ� ������ ��
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

    // ������ ����ϴ� �Լ�
    public void UseCoin(int useCoinCnt)
    {
        // �ش� �Լ��� ����ϱ� ���� �ʿ��� ������ ���� ������� Ȯ���ϰ� �� �Լ��� �������ִ� ��
        coinCount = Mathf.Clamp(coinCount - useCoinCnt, 0, maxCoinCount);

        // ������ �����ϰ�, 
        SaveCoinData();
        // UI Update �ϱ�
        onChangeCoinCnt?.Invoke(coinCount);
    }

    // ������ �޴� �Լ�
    public void RecieveCoin(int reciveCoinCnt)
    {
        coinCount = Mathf.Clamp(coinCount + reciveCoinCnt, 0, maxCoinCount);

        // ������ �����ϰ�, 
        SaveCoinData();
        // UI Update �ϱ�
        onChangeCoinCnt?.Invoke(coinCount);
    }

    #endregion
}

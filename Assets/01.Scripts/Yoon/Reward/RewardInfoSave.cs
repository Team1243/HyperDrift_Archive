using UnityEngine;
using System;

public class RewardInfoSave : MonoBehaviour
{
    [SerializeField] private RewardInfoListSO reardInfoList;

    private string saveKey = "rewardInfo";

    private void OnEnable()
    {
        Load();
    }

    private void OnDisable()
    {
        Save();
    }

    public void Save()
    {
        string saveData = "0";
        int num = 0;
        foreach (var info in reardInfoList.RewardInfoList)
        {
            num = Convert.ToInt32(info.isRecieve);
            saveData += " " + num;
        }

        PlayerPrefs.SetString(saveKey, saveData);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string loadData = PlayerPrefs.GetString(saveKey);
            string[] booleanList = loadData.Split(' ');

            for (int i = 1; i < booleanList.Length; i++)
            {
                int num = Convert.ToInt32(booleanList[i]);
                if (num == 1)
                {
                    reardInfoList.RewardInfoList[i - 1].isRecieve = true;
                }
                else
                {
                    reardInfoList.RewardInfoList[i - 1].isRecieve = false;
                }
            }
        }
        else
        {
            Save();
        }
    }
}

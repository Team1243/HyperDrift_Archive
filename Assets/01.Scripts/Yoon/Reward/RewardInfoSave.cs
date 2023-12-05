using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RewardInfoSave : MonoBehaviour
{
    private static RewardInfoSave _instance;
    public static RewardInfoSave Intance => _instance;

    [SerializeField] private RewardInfoListSO reardInfoList;
    private string _savePath;

    private List<string> tempRewardInfoList = new List<string>();

    private bool isLoaded = false;
    public bool IsLoaded => isLoaded;

    private void Awake()
    {
        _instance = this;
        
        if (Application.platform == RuntimePlatform.Android)
        {
            _savePath = Application.dataPath + "/SaveData/";
        }
        else
        {
            _savePath = Application.persistentDataPath + "/SaveData/";
        }

        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        foreach (var rewardInfo in reardInfoList.RewardInfoList)
        {
            string json = JsonUtility.ToJson(rewardInfo);
            tempRewardInfoList.Add(json);
        }

        Load();
    }

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
        string finalJson = JsonUtility.ToJson(tempRewardInfoList);
        File.WriteAllText(_savePath + "RewardInfoList.json", finalJson);
    }

    public void Load()
    {
        if (File.Exists(_savePath + "RewardInfoList.json"))
        {
            string json = File.ReadAllText(_savePath + "RewardInfoList.json");

            tempRewardInfoList = JsonUtility.FromJson<List<string>>(json);

            foreach (var r in tempRewardInfoList)
            {
                RewardInfo rewardInfo = JsonUtility.FromJson<RewardInfo>(r);
                reardInfoList.RewardInfoList.Add(rewardInfo);
            }

            isLoaded = true;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestSave : MonoBehaviour
{
    public static QuestSave Instance = null;

    [SerializeField] private QuestInfoListSO questListSO;
    private string savePath;
    private Dictionary<string, Quest> saveFile = new Dictionary<string, Quest>();

    private void Awake()
    {
        Instance = this;

        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.dataPath + "/SaveData/Quest/";
        }
        else
        {
            savePath = Application.persistentDataPath + "/SaveData/Quest/";
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        foreach (var saveData in questListSO.QuestList)
        {
            saveFile.Add(saveData.Id, saveData);
        }

        Load();
    }

    private void OnEnable()
    {
        Save();
    }

    private void OnDisable()
    {
        Load();
    }

    // 전체 SO 저장
    public void Save()
    {
        foreach (var str in saveFile.Keys)
        {
            string questJson = JsonUtility.ToJson(saveFile[str]);
            string jobJson = JsonUtility.ToJson(saveFile[str].Job);
            File.WriteAllText(savePath + saveFile[str].Id + "Quest.json", questJson);
            File.WriteAllText(savePath + saveFile[str].Id + "Job.json", jobJson);
        }
    }

    // 특정 SO만 저장
    public void Save(string str)
    {
        string questJson = JsonUtility.ToJson(saveFile[str]);
        string jobJson = JsonUtility.ToJson(saveFile[str].Job);
        File.WriteAllText(savePath + str + "Quest.json", questJson);
        File.WriteAllText(savePath + str + "Job.json", jobJson);
    }

    public void Load()
    {
        foreach (var str in saveFile.Keys)
        {
            if (File.Exists(savePath + saveFile[str].Id + "Quest.json") == false)
            {
                continue;
            }

            string questJson = File.ReadAllText(savePath + saveFile[str].Id + "Quest.json");
            string jobJson = File.ReadAllText(savePath + saveFile[str].Id + "Job.json");
            
            Quest data = JsonUtility.FromJson<Quest>(questJson);
            {
                saveFile[str].Id = data.Id;
                saveFile[str].DisplayName = data.DisplayName;
                saveFile[str].Description = data.Description;
                saveFile[str].State = data.State;

                // Job
                Job tempJob = JsonUtility.FromJson<Job>(jobJson);
                {
                    saveFile[str].Job.GoalProgressValue = tempJob.GoalProgressValue;
                    saveFile[str].Job.CurrentProgressValue= tempJob.CurrentProgressValue;
                }
                saveFile[str].Job = tempJob; 
                
                // RewardInfo
                RewardInfo tempRewardInfo = data.GetRewardInfo();
                saveFile[str].SetRewardInfo(tempRewardInfo);

                if (data.IsRunning == false)
                {
                    data.OnRunning();
                }
            }
        }
    }
}
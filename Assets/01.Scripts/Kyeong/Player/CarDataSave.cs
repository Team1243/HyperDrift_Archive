using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarDataSave : MonoBehaviour
{
    private static CarDataSave _instance;
    public static CarDataSave Instance => _instance;
    
    [SerializeField] private CarDataListSO _carDataList;
    private string _savePath;
    private Dictionary<string, CarData> _saveFileDic = new Dictionary<string, CarData>();
    [HideInInspector] public bool IsLoaded = false;

    private void Awake()
    {
        _instance = this;
        
        #if UNITY_EDITOR
        _savePath = Application.dataPath + "/SaveData/Car/";
        #elif UNITY_ANDROID
        _savePath = Application.persistentDataPath + "/SaveData/Car/";
        #endif
        if (!Directory.Exists(_savePath))
            Directory.CreateDirectory(_savePath);

        foreach (var saveData in _carDataList.CarDataList)
            _saveFileDic.Add(saveData.CarModel.name, saveData);
        
        Save();
        Load();
    }

    private void OnDisable()
    {
        Save();
    }

    /// <summary>
    /// SO 전체 저장
    /// </summary>
    public void Save()
    {
        foreach (var str in _saveFileDic.Keys)
        {
            string json = JsonUtility.ToJson(_saveFileDic[str]);
            File.WriteAllText(_savePath + _saveFileDic[str].CarModel.name + ".json", json);
        }
    }

    /// <summary>
    /// 특정 SO만 저장
    /// </summary>
    /// <param name="str">SO CarModel.name의 값을 넣어주세요.</param>
    public void Save(string str)
    {
        string json = JsonUtility.ToJson(_saveFileDic[str]);
        File.WriteAllText(_savePath + str + ".json", json);
    }

    /// <summary>
    /// 저장된 값들을 불러오는 것 
    /// </summary>
    public void Load()
    {
        foreach (var str in _saveFileDic.Keys)
        {
            if (!File.Exists(_savePath + _saveFileDic[str].CarModel.name + ".json"))
                continue;

            string json = File.ReadAllText(_savePath + _saveFileDic[str].CarModel.name + ".json");
            CarDataC data = JsonUtility.FromJson<CarDataC>(json);
            {
                _saveFileDic[str].CarClass = data.CarClass;
                _saveFileDic[str].CarName = data.CarName;
                _saveFileDic[str].CarModel = data.CarModel;
                _saveFileDic[str].CarMass = data.CarMass;
                _saveFileDic[str].Acceleration = data.Acceleration;
                _saveFileDic[str].TurnSpeed = data.TurnSpeed;
                _saveFileDic[str].MaxSpeed = data.MaxSpeed;
                _saveFileDic[str].Price = data.Price;
                _saveFileDic[str].Fuel = data.Fuel;
                _saveFileDic[str].RayDistance = data.RayDistance;
                _saveFileDic[str].ColliderRadius = data.ColliderRadius;
                _saveFileDic[str].ColliderCenter = data.ColliderCenter;
                _saveFileDic[str].IsRock = data.IsRock;
            }
        }

        IsLoaded = true;
    }
}

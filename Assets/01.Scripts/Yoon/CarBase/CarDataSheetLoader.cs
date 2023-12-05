using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using UnityEngine;

public class CarDataSheetLoader : MonoBehaviour
{
    private HttpClient client = new HttpClient();

    private const string url = "https://docs.google.com/spreadsheets/d/1yRGM5xgH_fEPJXdLcrSw3rjWP7E6-uIU19FZq-0bJzM/export?format=tsv&range=A2:L19";

    [SerializeField] private List<GameObject> carModelList = new List<GameObject>();

    [SerializeField] private CarDataListSO carDataListSO;

    #if UNITY_EDITOR

    [ContextMenu("LoadData")]
    public async Task LoadDataAsync()
    {
        try
        {
            Debug.Log("data loading...");
            string data = await client.GetStringAsync(url);
            Debug.Log("data loading complete !!!");
            SetTextOnData(data);
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("HTTP 요청 오류: " + e.Message);
        }
    }

    private void SetTextOnData(string tsv)
    {
        // 가로 기준으로 나누기
        string[] row = tsv.Split('\n');
        // 열 = 세로의 양
        int rowSize = row.Length;
        // 오 = 가로의 양
        int columSize = row[0].Split('\t').Length;

        string[] column = new string[columSize - 1];

        carDataListSO.CarDataList.Clear();

        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');

            CarData carDataTemp = ScriptableObject.CreateInstance<CarData>();

            carDataTemp.CarName = column[0];
            carDataTemp.CarClass = (CarRateClass)int.Parse(column[1]);
            carDataTemp.CarModel = carModelList.Find((x) => x.name == column[2]);
            carDataTemp.CarMass = float.Parse(column[3]);
            carDataTemp.Acceleration = float.Parse(column[4]);
            carDataTemp.TurnSpeed = float.Parse(column[5]);
            carDataTemp.MaxSpeed = float.Parse(column[6]);
            carDataTemp.Fuel = int.Parse(column[7]);
            carDataTemp.Price = int.Parse(column[8]);
            carDataTemp.RayDistance = float.Parse(column[9]);
            carDataTemp.ColliderRadius = float.Parse(column[10]);

            var colVec = column[11].Split(',');
            carDataTemp.ColliderCenter.x = float.Parse(colVec[0]);
            carDataTemp.ColliderCenter.y = float.Parse(colVec[1]);
            carDataTemp.ColliderCenter.z = float.Parse(colVec[2]);
            
            carDataListSO.CarDataList.Add(carDataTemp);
            CreateCarDataAsset(carDataTemp);
        }
        Debug.Log("ScriptableObject 에셋으로 저장되었습니다. \n 경로 : Assets/07.ScriptableObjects/Yoon/Cardatas/");
    }

    private void CreateCarDataAsset(CarData carData)
    {
        string assetPath = "Assets/07.ScriptableObjects/Yoon/Cardatas/" + carData.CarModel.gameObject.name + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(carData, assetPath);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    #endif
}

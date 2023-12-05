using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "SO/Player/CarDataListSO")]
public class CarDataListSO : ScriptableObject
{
    public List<CarData> CarDataList = new();
}

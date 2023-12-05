using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum CarRateClass
{
    None = 0,
    Common = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4
}

[CreateAssetMenu (fileName = "CarData", menuName = "SO/Player/CarData")] 
public class CarData : ScriptableObject
{
    //자동차 클래스
    public CarRateClass CarClass = CarRateClass.None;

    //자동차 이름
    public string CarName = "";

    //자동차 모델 프리팹
    public GameObject CarModel = null;
    
    //자동차 질량
    public float CarMass = 1;

    //자동차 가속도 
    public float Acceleration = 3000;
    
    //회전 속도
    public float TurnSpeed = 5;
    
    //자동차의 최대 속력
    public float MaxSpeed = 40;

    //자동차 가격
    public int Price = 10000;

    //자동차 연료
    public float Fuel = 3;

    //(내부)자동차가 바닥에 닿았는지 체크하기 위한 거리 변수
    public float RayDistance = 1.1f;
    
    //(내부)자동차 콜라이더 Radius값
    public float ColliderRadius = 0.5f;

    //(내부)자동차 콜라이더 위치 값
    public Vector3 ColliderCenter = new Vector3(0, .45f, 0); 
    
    //(내부) 자동차가 해금된 상태인지 아닌지 확인하는 변수
    public bool IsRock = false;

    //자동차 해금된 상태인지 아닌지 확인하는 함수
    public void UnlockThisCar() => IsRock = true;
}

[Serializable]
public class CarDataC
{
    public CarRateClass CarClass = CarRateClass.None;
    public string CarName = "";
    public GameObject CarModel = null;
    public float CarMass = 1;
    public float Acceleration = 3000;
    public float TurnSpeed = 5;
    public float MaxSpeed = 40;
    public int Price = 10000;
    public float Fuel = 3;
    public float RayDistance = 1.1f;
    public float ColliderRadius = 0.5f;
    public Vector3 ColliderCenter = new Vector3(0, .45f, 0);
    public bool IsRock = false;
}

// CarData Excel
// https://docs.google.com/spreadsheets/d/1yRGM5xgH_fEPJXdLcrSw3rjWP7E6-uIU19FZq-0bJzM/edit#gid=0


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
    //�ڵ��� Ŭ����
    public CarRateClass CarClass = CarRateClass.None;

    //�ڵ��� �̸�
    public string CarName = "";

    //�ڵ��� �� ������
    public GameObject CarModel = null;
    
    //�ڵ��� ����
    public float CarMass = 1;

    //�ڵ��� ���ӵ� 
    public float Acceleration = 3000;
    
    //ȸ�� �ӵ�
    public float TurnSpeed = 5;
    
    //�ڵ����� �ִ� �ӷ�
    public float MaxSpeed = 40;

    //�ڵ��� ����
    public int Price = 10000;

    //�ڵ��� ����
    public float Fuel = 3;

    //(����)�ڵ����� �ٴڿ� ��Ҵ��� üũ�ϱ� ���� �Ÿ� ����
    public float RayDistance = 1.1f;
    
    //(����)�ڵ��� �ݶ��̴� Radius��
    public float ColliderRadius = 0.5f;

    //(����)�ڵ��� �ݶ��̴� ��ġ ��
    public Vector3 ColliderCenter = new Vector3(0, .45f, 0); 
    
    //(����) �ڵ����� �رݵ� �������� �ƴ��� Ȯ���ϴ� ����
    public bool IsRock = false;

    //�ڵ��� �رݵ� �������� �ƴ��� Ȯ���ϴ� �Լ�
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


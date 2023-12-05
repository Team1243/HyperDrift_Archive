using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CarUI
{
    #region variable
    private CarData _carData;
    private CarRateClass _carClass = CarRateClass.None;
    private string _carName = "";
    private GameObject _carModel = null;
    private float _acceleration = 175000;
    private float _turnSpeed = 5;
    private float _mxSpeed = 35;
    private int _price = 10000;
    public int Price => _price;
    private float _fuel = 3;
    private bool _isRock = false;
    public bool Isrock => _isRock;
    #endregion

    #region names
    //Label Name
    private readonly string m_carName_Label = "carName-label";
    private readonly string m_carCalss_Label = "carClass-label";
    private readonly string m_carPrice_Label = "CarPirce-label";

    //ProgressBar Name
    private readonly string m_MaxSpeed_Bar = "MaxSpeed-ProgressBar";
    private readonly string m_Acceleration_Bar= "Acceleration-ProgressBar";
    private readonly string m_TurnSpeed_Bar = "TurnSpeed-ProgressBar";
    private readonly string m_Durability_Bar = "Durability-ProgressBar";

    //VisualElement Name
    private readonly string m_CarPrice_container = "bottom-carBuy-VisualElement";

    //Button Name
    private readonly string m_SelectButton = "select-button";

    //Class Name
    private readonly string c_ButtonLock = "lock";
    #endregion

    #region UI
    //Label
    private Label k_carName_Label;
    private Label k_carCalss_Label;
    private Label k_carPrice_Label;

    //ProgressBar
    private ProgressBar k_MaxSpeed_Bar;
    private ProgressBar k_Acceleration_Bar;
    private ProgressBar k_TurnSpeed_Bar;
    private ProgressBar k_Durability_Bar;

    //VisualElement
    private VisualElement k_CarPrice_container;

    //Button
    private Button k_SelectButton;
    #endregion

    private GameObject carObj;

    public CarUI(VisualElement root, CarData carData) 
    {
        _carData = carData;
        _carClass = carData.CarClass;
        _carName = carData.CarName;
        _carModel = carData.CarModel;
        _acceleration = carData.Acceleration;
        _turnSpeed = carData.TurnSpeed;
        _mxSpeed = carData.MaxSpeed;
        _price = carData.Price;
        _fuel = carData.Fuel;
        _isRock = carData.IsRock;


        SetVisualElements(root);
        Setup(carData.CarModel);
    }

    public void BuyCar()
    {
        k_CarPrice_container.style.display = DisplayStyle.None;
        k_SelectButton.RemoveFromClassList(c_ButtonLock);
        _carData.UnlockThisCar();
    }

    public void OnEnter()
    {
        carObj.SetActive(true);
        k_carName_Label.text = _carName;

        string CarClass = " ";

        switch (_carClass)
        {
            case CarRateClass.None:
                CarClass = "None";
                break;
            case CarRateClass.Common:
                CarClass = "Common";
                break;
            case CarRateClass.Rare:
                CarClass = "Rare";
                break;
            case CarRateClass.Epic:
                CarClass = "Epic";
                break;
            case CarRateClass.Legendary:
                CarClass = "Legendary";
                break;
        }

        k_carCalss_Label.text = CarClass;


        k_MaxSpeed_Bar.value = _mxSpeed;
        k_Durability_Bar.value = _fuel;
        k_TurnSpeed_Bar.value = _turnSpeed;
        k_Acceleration_Bar.value = _acceleration;

        if (_isRock)
        {
            k_CarPrice_container.style.display = DisplayStyle.Flex;
            k_carPrice_Label.text = $"x{_price.ToString()}";
            k_SelectButton.AddToClassList(c_ButtonLock);
        }
        else
        {
            k_CarPrice_container.style.display = DisplayStyle.None;
            k_SelectButton.RemoveFromClassList(c_ButtonLock);
        }
    }

    public void OnExit()
    {
        carObj.SetActive(false);
    }

    private void Setup(GameObject obj)
    {
        carObj = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity);

        for(int i = 0; i < carObj.transform.childCount; i++)
        {
            GameObject.Destroy(carObj.transform.GetChild(i).gameObject.GetComponent<WheelSkid>());
        }
        carObj.transform.parent = GameObject.Find("CarPivot").transform;
        carObj.transform.localPosition = Vector3.zero;
        carObj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        carObj.SetActive(false);
    }

    private void SetVisualElements(VisualElement root)
    {
        k_carName_Label = root.Q<Label>(m_carName_Label);
        k_carCalss_Label = root.Q<Label>(m_carCalss_Label);
        k_carPrice_Label = root.Q<Label>(m_carPrice_Label);

        k_MaxSpeed_Bar = root.Q<ProgressBar>(m_MaxSpeed_Bar);
        k_Acceleration_Bar = root.Q<ProgressBar>(m_Acceleration_Bar);
        k_TurnSpeed_Bar = root.Q<ProgressBar>(m_TurnSpeed_Bar);
        k_Durability_Bar = root.Q<ProgressBar>(m_Durability_Bar);

        k_CarPrice_container = root.Q<VisualElement>(m_CarPrice_container);
        k_CarPrice_container.style.display = DisplayStyle.None;

        k_SelectButton = root.Q<Button>(m_SelectButton);
    }
}

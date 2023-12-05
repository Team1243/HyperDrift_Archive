using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class garageScreen : MenuScreen
{
    [SerializeField] private CoinSystem coinSystem;
    [SerializeField] private CarDataListSO carData;
    [SerializeField] private Transform carPos;

    private List<CarUI> _carList = new List<CarUI>();
    private int _curIndex = 0;
    [Tooltip("현재 화면에 보이는 자동차")]private CarUI _curCarUI;
    [Tooltip("현재 선택된 자동차")]private CarUI _curSelectedCar;

    [SerializeField][Range(1f, 30f)] private float carRotateSpeed = 3f;


    //Buttons Name
    private readonly string k_carLeftButton = "car_left-button";
    private readonly string k_carRightButton = "car_right-button";
    private readonly string k_selectButton = "select-button";

    //class 
    private readonly string c_onLocked = "lock--active";

    //Label Name
    private readonly string k_CarSelectLabel = "select-label";

    //VisualElements Name
    private readonly string k_GarageVisualElement = "garage-VisualElement";
    private readonly string k_CarPriceVisualElement = "bottom-carBuy-VisualElement";

    //VisualElements
    private VisualElement m_GarageVisualElement;
    private VisualElement m_CarPriceVisualElement;

    //Buttons
    private Button m_carLeftButton;
    private Button m_carRightButton;
    private Button m_selectButton;
    private Button Test;

    //Labels
    private Label m_CarSelectLabel;

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        m_GarageVisualElement = m_Root.Q<VisualElement>(k_GarageVisualElement);
        m_CarPriceVisualElement = m_Root.Q<VisualElement>(k_CarPriceVisualElement);

        m_carLeftButton = m_Root.Q<Button>(k_carLeftButton);
        m_carRightButton = m_Root.Q<Button>(k_carRightButton);
        m_selectButton = m_Root.Q<Button>(k_selectButton);
        m_CarSelectLabel = m_Root.Q<Label>(k_CarSelectLabel);

        Test = m_Root.Q<Button>("LeftButton");


        CarSetup();
        UpdateCar();
    }


    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();


        m_carLeftButton.RegisterCallback<ClickEvent>(evt =>
        {
            --_curIndex;
            UpdateCar();
        });

        m_carRightButton.RegisterCallback<ClickEvent>(evt =>
        {
            ++_curIndex;
            UpdateCar();
        });

        m_selectButton.RegisterCallback<ClickEvent>(SelectCar);
        m_CarPriceVisualElement.RegisterCallback<ClickEvent>(BuyCar);
    }

    private void BuyCar(ClickEvent evt)
    {
        if (!coinSystem.IsCoinEnough(_curCarUI.Price))
            return;
        coinSystem.UseCoin(_curCarUI.Price);
        _curCarUI.BuyCar();
    }

    private void SelectCar(ClickEvent evt)
    {
        if (_curCarUI.Isrock)
            return;
        //if (carData.CarDataList[_curIndex].IsRock) return;
        //Debug.Log("SelectButton Active");
        _curSelectedCar = _curCarUI;
        UpdateCar();
        PlayerPrefs.SetInt("car", _curIndex);
    }



    private void CarSetup()
    {
        foreach(var car in carData.CarDataList)
        {
            CarUI carUI = new CarUI(m_Screen, car);
            _carList.Add(carUI);
            //GameObject carObj = Instantiate(car.CarModel, Vector3.zero, Quaternion.identity);
            //carObj.transform.parent = carPos;
            //carObj.transform.localPosition = Vector3.zero;
            //carObj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            //carObj.SetActive(false);
        }
        if(_carList.Count < 0)
        {
            Debug.LogError("_carList is null!");
            return;
        }
        _curCarUI = _carList[0];
        _curCarUI.OnEnter();

        _curSelectedCar = _carList[PlayerPrefs.GetInt("car")];
    }

    private void UpdateCar()
    {
        if (_curIndex > _carList.Count - 1)
            _curIndex = 0;
        else if (_curIndex < 0)
            _curIndex = _carList.Count - 1;


        _curCarUI.OnExit();
        _curCarUI = _carList[_curIndex];
        _curCarUI.OnEnter();


        if(_curSelectedCar == _curCarUI)
        {
            m_CarSelectLabel.text = "탑승중";
            m_selectButton.style.unityBackgroundImageTintColor = Color.green;
        }
        else
        {
            m_selectButton.style.unityBackgroundImageTintColor = Color.white;
            m_CarSelectLabel.text = "탑승";
        }
    }

    private void Update()
    {
        carPos.transform.Rotate(Vector3.up * carRotateSpeed * Time.deltaTime);
    }
}

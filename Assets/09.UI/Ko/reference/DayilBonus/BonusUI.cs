using MyUILibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BonusState
{
    None, CanRecieve, Recieved
}

public class BonusUI
{
    int _index;
    public VisualElement _root;
    private VisualElement _bonusPanel;
    private RewardType RewardType = RewardType.None;
    private Sprite _sprite;
    private int _rewardAmount = 0;
    private BonusState _bonusState = BonusState.None;
    private RewardInfo _rewardInfo;

    public BonusUI(int index, VisualElement root, RewardInfo rewardInfo, BonusState bonusState = BonusState.None)
    {
        _root = root;
        _rewardInfo = rewardInfo;
        _bonusPanel = root.Q<DailyBonusVisualElement>("DailyBonus-panel");
        RewardType = rewardInfo.RewardType;
        _rewardAmount = rewardInfo.RewardAmount;
        _index = index;


        _bonusState = bonusState;
        if (_bonusState == BonusState.Recieved)
            _bonusPanel.Q<VisualElement>("rewardReceived-container").AddToClassList("received");
        else if (_bonusState == BonusState.CanRecieve)
            _bonusPanel.Q<VisualElement>("rewardFrame").style.display = DisplayStyle.Flex;


        _bonusPanel.RegisterCallback<ClickEvent>(RewardItem);

        ResouceLoad(rewardInfo.RewardType);
        _bonusPanel.Q<VisualElement>("reward-icon").style.backgroundImage = new StyleBackground(_sprite);

        _bonusPanel.Q<Label>("reward-text").text = "+" + _rewardAmount.ToString();
        _bonusPanel.Q<Label>("day-text").text = $"{index + 1}¿œ";
    }

    private void RewardItem(ClickEvent evt)
    {
        if (_bonusState != BonusState.CanRecieve)
            return;

        _bonusState = BonusState.Recieved;

        DailyBonusPopup.OnRecieveReward?.Invoke(_rewardInfo);
        DailyBonusPopup.OnUpdateInfo?.Invoke();
        DailyBonusPopup.IsDayRecieve = true;

        _bonusPanel.Q<VisualElement>("rewardFrame").style.display = DisplayStyle.None;
        _bonusPanel.Q<VisualElement>("rewardReceived-container").AddToClassList("received");
    }

    private void ResouceLoad(RewardType rewardType)
    {
        string path = "sprites/";
        path += rewardType == RewardType.Coin ? "Icon_Golds" : "Icon_Heart";
        // Debug.Log(path);    
        _sprite = Resources.Load<Sprite>(path); 
    }
}

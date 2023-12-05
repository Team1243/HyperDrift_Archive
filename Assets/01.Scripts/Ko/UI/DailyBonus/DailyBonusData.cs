using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Coin, Heart
}

[CreateAssetMenu(fileName = "new DailyBonusData", menuName = "SO/UI/DailyBonusData")]
public class DailyBonusData : ScriptableObject
{
    [SerializeField] private int m_itemCount;
    public int m_ItemCount => m_itemCount;

    [SerializeField] private ItemType m_type;
    public ItemType m_Type => m_type;

    public Sprite m_Sprite { get; private set; }


    private void OnValidate()
    {
        string name = null;

        switch (m_type)
        {
            case ItemType.Coin:
                name = "Icon_Golds";
                break;
            case ItemType.Heart:
                name = "Icon_Heart";
                break;
        }

        m_Sprite = Resources.Load<Sprite>("sprites/" + name);
    }

}

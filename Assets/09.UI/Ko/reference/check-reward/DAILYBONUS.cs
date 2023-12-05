//using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyUILibrary
{
    public class DAILYBONUS : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DAILYBONUS, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits { }


        private Label m_dayText;
        public VisualElement a;


        //public void DAILYBONUS(string rewardName, int rewardCount, Image rewardIcon)
        //{
        //    m_dayText = new Label();
        //    a = new();
        //    VisualElement.Add
        //    }
    }
}

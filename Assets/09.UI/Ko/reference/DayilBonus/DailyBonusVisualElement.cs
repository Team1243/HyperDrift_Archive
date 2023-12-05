using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyUILibrary
{
    public class DailyBonusVisualElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DailyBonusVisualElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_panelName =
                new UxmlStringAttributeDescription { name = "Bonus-name", defaultValue = "" };

            private UxmlIntAttributeDescription m_panelIndex = new UxmlIntAttributeDescription
            { name = "Bonus-index", defaultValue = 0 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as DailyBonusVisualElement;

                ate.bonusName = m_panelName.GetValueFromBag(bag, cc);
                ate.bonusIndex = m_panelIndex.GetValueFromBag(bag, cc);
            }
        }

        public string bonusName { get; set; }
        public int bonusIndex { get; set; }
    }
}

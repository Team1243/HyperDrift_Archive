using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyUILibrary
{
    public class CustomProgressBar : VisualElement
    {
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_ProgressAttribute = new UxmlFloatAttributeDescription()
            {
                name = "progress"
            };
        }

    }
}


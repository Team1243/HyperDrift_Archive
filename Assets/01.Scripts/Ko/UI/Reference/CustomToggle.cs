using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomToggle : MonoBehaviour
{
    private VisualElement m_Root;
    private VisualElement m_Panel;
    private VisualElement m_Toggle;

    private void Start()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Panel = m_Root.Q<VisualElement>("unity-checkmark");
        AddElement();
    }

    private void AddElement() 
    {
        m_Toggle = new VisualElement();
        m_Panel.Add(m_Toggle);
        m_Toggle.name = "Toggle";
        m_Toggle.AddToClassList("toggle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

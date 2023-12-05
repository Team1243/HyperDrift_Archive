using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PopupPanel : MonoBehaviour
{
    [Tooltip("this popup name")]
    [SerializeField] protected string m_CurScreenName;
    [SerializeField] protected string m_PopupName;

    [SerializeField] protected MainUIManager m_MainUI;
    [SerializeField] protected UIDocument m_Document;

    protected VisualElement m_CurScrene;
    protected VisualElement m_Popup;
    protected VisualElement m_Root;

    public event Action OnScreenStarted;
    public event Action OnScreenEnd;

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_PopupName))
            m_PopupName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        if (m_MainUI == null)
            m_MainUI = GetComponent<MainUIManager>();

        if (m_Document == null)
            m_Document = GetComponent<UIDocument>();

        if (m_Document == null && m_MainUI != null)
            m_Document = m_MainUI.MainMenuDocument;

        if (m_Document == null)
        {
            Debug.LogWarning("MenuScreen " + m_CurScreenName + ": missing UIDocument. Check Script Execution Order.");
            return;
        }
        else
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }
    }

    protected virtual void SetVisualElements()
    {
        if (m_Document != null)
            m_Root = m_Document.rootVisualElement;

        m_CurScrene = GetVisualElement(m_CurScreenName);
        m_Popup = GetVisualElement(m_PopupName);
    }

    protected virtual void RegisterButtonCallbacks()
    {

    }

    public VisualElement GetVisualElement(string elementName)
    {
        if (string.IsNullOrEmpty(elementName) || m_Root == null)
            return null;

        return m_Root.Q(elementName);
    }

    public bool IsVisible()
    {
        if (m_CurScrene == null)
            return false;

        return (m_CurScrene.style.display == DisplayStyle.Flex);
    }

    protected enum Visible { show, hide}

    protected virtual void SetScreenVisible(Visible visible = Visible.show)
    {
        m_Popup.style.display = visible == Visible.show ? DisplayStyle.Flex : DisplayStyle.None;

        if (visible == Visible.show)
            OnScreenStarted?.Invoke();
        else
            OnScreenEnd?.Invoke();
    }


    public abstract void ShowScreenRoutine();


    public abstract void HideScreenRoutine();

}

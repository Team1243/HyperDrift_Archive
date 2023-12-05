using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class title : MonoBehaviour
{
    private UIDocument document;
    private readonly string Mixer_Master = "MasterVolume";
    private readonly string Mixer_Music = "MusicVolume";
    private readonly string Mixer_SFX = "SFXVolume";
    private void Awake()
    {
            PlayerPrefs.SetFloat(Mixer_Master, 1);
            PlayerPrefs.SetFloat(Mixer_Music, 1);
            PlayerPrefs.SetFloat(Mixer_SFX, 1);

        document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        VisualElement backgorund = root.Q<VisualElement>("background");
        backgorund.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("SceneLoad");
            SceneManager.LoadScene("Home");
        });
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene("Home");
    }
}

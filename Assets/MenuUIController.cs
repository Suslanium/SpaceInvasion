using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _settingsElementsTemplate;
    private VisualElement _menuButtonsContainer;
    private VisualElement _settingsElements;
    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;
    private VisualElement _settingsContainer;
    private SliderInt _soundSlider;
    private SliderInt _musicSlider;
    private Button _backButton;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _startButton = root.Q<Button>("StartButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _exitButton = root.Q<Button>("ExitButton");
        _menuButtonsContainer = root.Q<VisualElement>("MenuButtonsContainer");

        _startButton.clicked += StartGame;
        _settingsButton.clicked += OnSettingsButtonClicked;
        _exitButton.clicked += ExitGame;

        _settingsElements = _settingsElementsTemplate.CloneTree();
        
        _backButton = _settingsElements.Q<Button>("BackButton");
        _soundSlider = _settingsElements.Q<SliderInt>("SoundSlider");
        _musicSlider = _settingsElements.Q<SliderInt>("MusicSlider");

        _backButton.clicked += OnBackButtonClicked;
    }

    private void OnSettingsButtonClicked()
    {
        _menuButtonsContainer.Clear();
        _menuButtonsContainer.Add(_settingsElements);
        _musicSlider.value = (int)(PlayerPrefs.GetFloat("MusicVolumeSettings", 1.0f) * 100);
        Debug.Log(PlayerPrefs.GetFloat("MusicVolumeSettings", 1.0f));
    }

    private void OnBackButtonClicked()
    {
        PlayerPrefs.SetFloat("MusicVolumeSettings", _musicSlider.value / 100.0f);
        _menuButtonsContainer.Clear();
        _menuButtonsContainer.Add(_startButton);
        _menuButtonsContainer.Add(_settingsButton);
        _menuButtonsContainer.Add(_exitButton);
    }
    
    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}

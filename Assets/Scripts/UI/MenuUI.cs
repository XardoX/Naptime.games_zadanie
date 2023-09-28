using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private MenuButton menuButtonPrefab;

    [SerializeField]
    private SegmentedControl objectCountSegmentedControl;

    [SerializeField]
    private CanvasGroup mainMenuWindow, endGameWindow;

    public Action<int> OnObjectsCountSelected;

    public Action OnStartButtonClicked;

    public void CreateMenuButtons(GameplaySettings gameplaySettings)
    {
        foreach (var objectsCountSetting in gameplaySettings.objectsCountSettings)
        {
            var newButton = Instantiate(menuButtonPrefab, objectCountSegmentedControl.transform);
            newButton.Icon.color = objectsCountSetting.buttonIconColor;
            newButton.Text.text = objectsCountSetting.count.ToString();
        }
    }

    public void StartGame() => OnStartButtonClicked?.Invoke();

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void ToggleMenuUI(bool toggle)
    {
        mainMenuWindow.gameObject.SetActive(toggle);
    }

    public void ToggleEndGameUI(bool toggle)
    {
        endGameWindow.gameObject.SetActive(toggle);
    }

    private void Awake()
    {
        objectCountSegmentedControl.onValueChanged.AddListener(OnObjectsCountSelected.Invoke);
    }
}

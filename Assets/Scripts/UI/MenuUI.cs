using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private MenuButton menuButtonPrefab;

    [SerializeField]
    private SegmentedControl objectCountSegmentedControl;

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

    public void ToggleUI(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    private void Awake()
    {
        objectCountSegmentedControl.onValueChanged.AddListener(OnObjectsCountSelected.Invoke);
    }
}

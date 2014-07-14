using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour 
{
    private Dictionary<UIAction, Action> _registeredUICallBacks 
        = new Dictionary<UIAction, Action>();

    private Dictionary<UILabelName, UILabel> _registeredUILabels 
        = new Dictionary<UILabelName, UILabel>();

    private Dictionary<UISliderName, UISlider> _registeredUISliders 
        = new Dictionary<UISliderName, UISlider>();

    public UIPanel gameStatsPanel;
    public UIPanel gameOverPanel;
	public GameObject joystickPanel;
    public GameObject actionButtonsPanel;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public UIPanel intermissionPanel;

    public void registerUICallbacks(UIAction uiAction,Action callback)
    {
        if (!_registeredUICallBacks.ContainsKey(uiAction))
            _registeredUICallBacks.Add(uiAction, callback);
        else
            Debug.LogError(string.Format("[{0}] {1} already registered. " +
                "Duplicated uiAction will not be registered.", GetType().FullName, uiAction));
    }

    public void registerUILabels(UILabelName uiLabelName, UILabel uiObj)
    {
        if (!_registeredUILabels.ContainsKey(uiLabelName))
            _registeredUILabels.Add(uiLabelName, uiObj);
        else
            Debug.LogError(string.Format("[{0}] {1} already registered. " +
                "Duplicated uiLabel will not be registered.", GetType().FullName, uiLabelName));
    }

    public void registerUISliders(UISliderName uiSliderName, UISlider uiObj)
    {
        if (!_registeredUISliders.ContainsKey(uiSliderName))
            _registeredUISliders.Add(uiSliderName, uiObj);
        else
            Debug.LogError(string.Format("[{0}] {1} already registered. " +
                "Duplicated uiSlider will not be registered.", GetType().FullName, uiSliderName));
    }

    public void onButtonClick(UIAction buttonAction)
    {
        if(_registeredUICallBacks.ContainsKey(buttonAction))
        {
            if (_registeredUICallBacks[buttonAction] != null)
                _registeredUICallBacks[buttonAction]();
        }
    }

    public void updateLabelText(UILabelName uiLabelName, string text)
    {
        if(_registeredUILabels.ContainsKey(uiLabelName))
        {
            _registeredUILabels[uiLabelName].text = text;
        }
    }

    public void updateSliderValue(UISliderName uiSliderName, float maxValue, float currentValue)
    {
        if(_registeredUISliders.ContainsKey(uiSliderName))
        {
            float fraction = currentValue / maxValue;
            _registeredUISliders[uiSliderName].value = fraction;
        }
    }

    public void showRestartPanel()
    {
        hideAll();
    }

    public void showGameOverPanel()
    {
        hideAll();
    }

    public void showGameStatsPanel(bool isShow)
    {
        hideAll();

        if (isShow)
            gameStatsPanel.alpha = 1;
    }

    public void showGameOverPanel(bool isShow)
    {
        hideAll();

        if (isShow)
            gameOverPanel.alpha = 1;
    }

    public void showIntermissionPanel(bool isShow)
    {
        hideAll();

        if (isShow)
            intermissionPanel.alpha = 1;
    }

    public void hideAll()
    {
        if (gameStatsPanel != null)
            gameStatsPanel.alpha = 0;

        if (gameOverPanel != null)
            gameOverPanel.alpha = 0;

        if(joystickPanel != null)
            joystickPanel.SetActive(false);

        if(actionButtonsPanel != null)
            actionButtonsPanel.SetActive(false);

        if(settingsPanel != null)
            settingsPanel.SetActive(false);

        if(mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (intermissionPanel != null)
            intermissionPanel.alpha = 0;
    }
}

public enum UIAction
{
    BUTTON_A,
    BUTTON_B,
    BUTTON_C,
    BUTTON_D,
    INTERMISSION_CONTINUE,
    BUTTON_PLAYAGAIN
}

public enum UILabelName
{
    UI_LIVES,
    UI_TIME,
    UI_GUNPOWDER,
    UI_DYNAMITE,
    UI_FUSE,
    UI_INTERMISSION_LIVES
}

public enum UISliderName
{
    UI_LIVES_SLIDER,
    UI_TIMER_SLIDER
}
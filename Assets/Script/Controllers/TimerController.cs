using UnityEngine;
using System;
using System.Collections;

public class TimerController : MonoBehaviour 
{
    public GameController gameController;
    public float speed;

    private float _startTime;
    private float _duration;
    private float _maxDuration;
    private bool _isCountDown = false;
    private Action _onTimeOut;

    public void initialize(float duration, Action onTimeOut)
    {
        _duration = duration;
        _maxDuration = duration;
        _startTime = Time.time;
        _onTimeOut = onTimeOut;
        _isCountDown = true;

        gameController.uiController.updateSliderValue(
            UISliderName.UI_TIMER_SLIDER,
            _maxDuration, _duration);
    }

    public void stop()
    {
        _isCountDown = false;
    }

    private void Update()
    {
        if(_isCountDown)
        {
            if ((Time.time - _startTime) > speed)
            {
                _duration--;
                gameController.uiController.updateSliderValue(
                    UISliderName.UI_TIMER_SLIDER,
                    _maxDuration, _duration);

                if(_duration <= 0)
                {
                    _isCountDown = false;

                    if(_onTimeOut != null)
                        _onTimeOut();
                }

                _startTime = Time.time;
            }
        }
    }
}
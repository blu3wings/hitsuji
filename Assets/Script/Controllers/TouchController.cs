using UnityEngine;
using System;
using System.Collections;

public class TouchController : MonoBehaviour 
{
    private Vector2 _startPos;
    private Action<float,Vector2> _onCallback;
    private Vector2 _direction;

    public void registerCallback(Action<float,Vector2> onCallback)
    {
        _onCallback = onCallback;
    }

	private void Update()
    {       
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _direction = touch.position - _startPos;
                    break;
                case TouchPhase.Ended:
                    if (_onCallback != null)
                        _onCallback(_direction.x, _direction.normalized);
                    break;
            }
        }
    }
}
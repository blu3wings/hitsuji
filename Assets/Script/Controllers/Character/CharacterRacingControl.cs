using UnityEngine;
using System.Collections;

public class CharacterRacingControl : MonoBehaviour 
{
    public float maxSpeed = 1.0f; //This determines the max speed the character can travel.
    public float acceleration = 1.0f; //This determine how fast can  the character reach max speed.
    public float standStillAccel = 1.0f;

    private float _startStandStillAccel;
    private bool _beginStandStillAccel = false;
    private bool _isAccelerate = false;
    private float _currentSpeed = 0;

    public void accelerate()
    {
        _isAccelerate = true;
    }

    public void boost()
    {

    }

    public void jump()
    {

    }

    public void FixedUpdate()
    {
        if(_beginStandStillAccel)
        {
            if((Time.time - _startStandStillAccel) > standStillAccel)
            {
                _currentSpeed += standStillAccel;
                _startStandStillAccel = Time.time;
            }
        }

        if(_currentSpeed >= maxSpeed)
        {
            _beginStandStillAccel = false;
        }
        else if(_currentSpeed <= 0)
        {
            _beginStandStillAccel = true;
        }

        if(_isAccelerate)
        {

        }

        transform.position += transform.right * _currentSpeed * Time.fixedDeltaTime;
    }
}
using UnityEngine;
using System.Collections;

public class CharacterController : CharacterBase 
{
    public Transform groundCheck;
    public CircleCollider2D collider;
    public string groundInteractableName;
    public float dashDuration = 1f;
    public float dashMultiplier = 1f;

	private GameController _gameController;
	private UIJoystick _joystick;
    private bool _isDashing = false;
    private float _startDashTime;
    private float _externalForce;

	private void Start()
	{
		if(_gameController == null)
		{
			//Lets make sure the GameController has been initialized
			if(GameObject.FindGameObjectWithTag("GameController")
			   .GetComponent<GameController>())
			{
				_gameController = GameObject.FindGameObjectWithTag("GameController")
					.GetComponent<GameController>();

				//Lets load the joystick control while we are at it
				_joystick = _gameController.uiController.joystickPanel
                    .GetComponentInChildren<UIJoystick>();

                _gameController.uiController.registerUICallbacks(UIAction.BUTTON_A, playerJump);
                _gameController.uiController.registerUICallbacks(UIAction.BUTTON_B, playerDash);
                _gameController.uiController.registerUICallbacks(UIAction.BUTTON_C, playerShoot);
                _gameController.uiController.registerUICallbacks(UIAction.BUTTON_D, playerThrow);
			}
		}
	}

	private void Update()
	{
        isGrounded = Physics2D.Linecast(transform.position, 
            groundCheck.position, 1 << LayerMask.NameToLayer(groundInteractableName));

        if(_joystick != null)
        {
            _direction = _joystick.position.x * _screenRight
            + _joystick.position.y * _screenForward;
        }	
	}

    /// <summary>
    /// Performs the jump action for the character.
    /// </summary>
    public void playerJump()
    {
        if (!isGrounded) return;

        rigidbody2D.AddForce(transform.up * jumpForce);      
    }

    /// <summary>
    /// Performs the dash action for the character.
    /// </summary>
    private void playerDash()
    {
        if (_isDashing) return; //Don't do anything if dashing is in progress

        if (!isGrounded) return; //Don't allow dash if character is not grounded. 

        _isDashing = true;

        _gameController.eventCallbackController
            .registerEvent(gameObject.name, dashDuration, false,onPlayerDashCompleted);
    }

    /// <summary>
    /// This gets invoke once the delay duration has been achieved;
    /// </summary>
    private void onPlayerDashCompleted(UnityEngine.Object obj)
    {
        _isDashing = false;
    }

    private void playerShoot()
    {
        
    }

    private void playerThrow()
    {
        
    }

	private void FixedUpdate()
	{
        if (isDead) return;

		float h = (Input.GetAxis("Horizontal")!=0?Input.GetAxis("Horizontal"):_direction.x);

        if(usePhysics)
        {
            if (h * rigidbody2D.velocity.x < maxSpeed)
                rigidbody2D.AddForce(Vector2.right * h * moveForce);

            if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
        }
        else
        {
            if (Mathf.Abs(h) > minSpeed)
            {
                if(_isDashing)
                    transform.position += transform.right * h * (maxSpeed * dashMultiplier) * Time.fixedDeltaTime;
                else
                    transform.position += transform.right * h * maxSpeed * Time.fixedDeltaTime;
            }
        }               

		if(h > 0 && facingRight)
		{
			flip();
		}
		else if (h < 0 && !facingRight)
		{
			flip();
		}
	}

	private void flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public override void dead()
    {
        isDead = true;
        playerJump();
        rigidbody2D.velocity = Vector2.zero;
        collider.isTrigger = true;
        _gameController.playerDied();
    }
}
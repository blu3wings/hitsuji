using UnityEngine;
using System.Collections;

public class CharacterController_Touch : CharacterBase 
{
    public Transform groundCheck;
    public CircleCollider2D collider;
    public string groundInteractableName;
    public TouchController touchController;
    public float moveDuration = 1f;

    private GameController _gameController;
    private float _force;
    private bool _isMove = false;

	public void initialize()
    {
        if(_gameController == null)
        {
            if(GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameController>())
            {
                _gameController = GameObject.FindGameObjectWithTag("GameController")
                    .GetComponent<GameController>();
            }
        }

        if (touchController != null)
            touchController.registerCallback(onMove);
    }

    private void onMove(float force,Vector2 direction)
    {
        _isMove = true;
        _direction = direction;       

        if (force < 0)
            force *= -1;

        force = Mathf.Clamp(force, 0, 300);
        force = force / 30;

        maxSpeed = force;

        moveDuration = force / 10;

        if (direction.y > 0.5f)
            jump();

        _gameController.eventCallbackController.
            registerEvent(gameObject.name, moveDuration, false, onStop);
    }

    private void onStop(UnityEngine.Object obj)
    {
        _isMove = false;
    }

    private void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position,
            groundCheck.position, 1 << LayerMask.NameToLayer(groundInteractableName));
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if(_isMove)
        {
            if (_direction.x * rigidbody2D.velocity.x < maxSpeed)
                rigidbody2D.AddForce(Vector2.right * _direction.x * moveForce);

            if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
                rigidbody2D.velocity = new Vector2(Mathf.Sign(
                    rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

            if (_direction.x > 0 && facingRight)
                flip();
            else if (_direction.x < 0 && !facingRight)
                flip();
        }
    }

    public void jump()
    {
        if (!isGrounded) return;

        rigidbody2D.AddForce(transform.up * jumpForce);
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
        jump();
        rigidbody2D.velocity = Vector2.zero;
        collider.isTrigger = true;
        _gameController.playerDied();
    }
}
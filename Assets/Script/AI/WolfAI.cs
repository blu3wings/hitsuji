using UnityEngine;
using System.Collections;

public class WolfAI : AIBehavior 
{
    private Collider2D[] _hits;
    private Collider2D[] _playerHits;

    private void initializeAI()
    {
        if (isAIInitialized) return;

        targetPos = new Vector2(transform.position.x +
            (facingDirection * flipRange), transform.position.y);

        move();

        isAIInitialized = true;
    }

    public override void move()
    {
        isMove = true;
    }    

    private void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position,
            groundCheck.position, 1 << LayerMask.NameToLayer(groundLayerName));

        if (isGrounded)
            initializeAI();
    }

    private void FixedUpdate()
    {
        if (!isAIInitialized) return;

        Physics2D.OverlapPointNonAlloc(frontCheck.position, _hits,
            LayerMask.NameToLayer(obstacleInteractableName));

        if (_hits != null)
        {
            //Check each of the colliders
            foreach (Collider2D c in _hits)
            {
                //If any of the colliders is an obstacle or ground
                if (c.tag == "Ground" || c.tag == "Obstacle")
                {
                    if (facingDirection == -1)
                        facingDirection = 1;
                    else if (facingDirection == 1)
                        facingDirection = -1;

                    //Flip the enemy to other direction
                    flip();

                    break;
                }
            }
        }

        //Creacte an array of all the colliders in front of the enemy
        Physics2D.OverlapPointNonAlloc(frontCheck.position, _playerHits,
            LayerMask.NameToLayer(playLayerName));

        if (_playerHits != null)
        {
            //Check each of the colliders
            foreach (Collider2D c in _playerHits)
            {
                //If any of the colliders is an obstacle or ground
                if (c.tag == "Player")
                {
                    //Invoke the player's dead sequence
                    if (!c.gameObject.GetComponent<CharacterController_Touch>().isDead)
                            c.GetComponent<CharacterController_Touch>().dead();

                    //Stop the enemy from moving
                    isMove = false;
                    break;
                }
            }
        }

        if (isMove)
        {
            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                if (facingDirection == -1)
                    facingDirection = 1;
                else if (facingDirection == 1)
                    facingDirection = -1;

                //Flip the enemy to other direction
                flip();
            }

            transform.position = Vector2.MoveTowards(transform.position,
                targetPos, maxSpeed * Time.fixedDeltaTime);
        }  
    }

    private void flip()
    {
        targetPos = new Vector2(transform.position.x +
            (facingDirection * flipRange), transform.position.y);

        //Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
using UnityEngine;
using System.Collections;

public class BirdAI : AIBehavior 
{
    /*
     * The bird has the ability to fly only in a straight line.
     * Generally they are more menacing compare to most of the
     * creatures due to the fact it can drop, well "dropping"
     * onto our hero.
     * 
     * Chains of birds can be deployed to create carpet bombing
     * mission. Now, this can get pretty interesting.
     */

    public bool allowFlip = false;
    public float newTempDistance;
    public float prevTempDistance;
    public float diveSpeed = 0;

    public Animator birdAnimator;
    public Animator birdEyeAnimator;

    private float _previousDistance;
    private GameController _gameController;

    public override void initializeAI()
    {
        move();
    }

    public override void registerBehavior(object obj)
    {
        _gameController = (GameController)obj;
    }

    public override void changeDirection(bool isForward)
    {
        if (isForward)
        {
            facingDirection = -1;
            Vector3 enemyScale = transform.localScale;
            enemyScale.x = 1;
            transform.localScale = enemyScale;
        }            
        else
            facingDirection = 1;
    }

    public override void move()
    {
        isMove = true;   
    }

    public override void freeze()
    {
        isMove = false;
    }

    private void Update()
    {
        if(player != null)
        {
            Vector2 playerVec = new Vector2(player.transform.position.x, 0);
            Vector2 birdVec = new Vector2(gameObject.transform.position.x, 0);

            float newDistance = Vector2.Distance(playerVec, birdVec);

            newTempDistance = newDistance;
            prevTempDistance = _previousDistance;

            if (_previousDistance == 0)
                _previousDistance = newDistance;

            if(_previousDistance > newDistance)
            {
                //Thsi means getting closer
                if (Vector2.Distance(playerVec, birdVec) < 10f)
                {
                    playerInRange();
                }
            }

            _previousDistance = newDistance;
        }
        else
        {
            if(isMove)
                player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void FixedUpdate()
    {
        if (!isMove) return;

        if (allowFlip)
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
        else
        {
            if (isDiving)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                targetPos, maxSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (facingDirection > 0)
                {
                    if (transform.position.x >=
                        _gameController.endStopper.transform.position.x)
                    {
                        Destroy(gameObject);
                    }
                }
                else if (facingDirection < 0)
                {
                    if (transform.position.x <=
                        _gameController.startStopper.transform.position.x)
                    {
                        Destroy(gameObject);
                    }
                }

                transform.Translate(facingDirection * maxSpeed * Time.fixedDeltaTime,
                0f, 0f, Space.World);
            }
        }        
    }

    protected void changeTarget()
    {
        targetPos = new Vector2(player.transform.position.x + (facingDirection * 10), 
            player.transform.position.y - 15);
        maxSpeed += diveSpeed;
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

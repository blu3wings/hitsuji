using UnityEngine;
using System;
using System.Collections;

public class Crates : RewardBase 
{
    public string groundLayerName;
    public BoxCollider2D collider;
    public Transform freeFallPoint;
    public Transform groundPoint;
    public float defaultSpeed;
    public float freeFallSpeed;

    public Animator crateAnimator;
    public Animator parachuteAnimator;

    private bool _isFreeFall = false;
    private bool _isGrounded = false;
    private bool _startFalling = false;
    private Action<Reward.RewardContent> _onPickUp;
    private GameController _gameController;

    public override void initialize(GameController gameController, Reward.RewardContent type,
        Action<Reward.RewardContent> onPickUp)
    {
        _onPickUp = onPickUp;
        _startFalling = true;

        rewardType = type;

        _gameController = gameController;
    }

    private void onTimesUp(UnityEngine.Object obj)
    {
        try
        {
            crateAnimator.SetBool("isTimesUp", true);

            _gameController.eventCallbackController.registerEvent(gameObject.name,
                disappearTime, false, destroy);
        }
        catch (Exception ex) { }        
    }

    public override void destroy(UnityEngine.Object obj)
    {
        _gameController.eventCallbackController.disposeEvent(gameObject.name);
        Destroy(gameObject);
    }

    private void Update()
    {
        //Starts the falling sequence
        if(_startFalling)
        {
             gameObject.transform.Translate(-Vector2.up * defaultSpeed * Time.deltaTime);
        }

        if(!_isFreeFall)
        {
            _isFreeFall = Physics2D.Linecast(gameObject.transform.position, 
                freeFallPoint.position, 1<< LayerMask.NameToLayer(groundLayerName));

            if(_isFreeFall)
            {
                crateAnimator.SetBool("isFall", true);
                parachuteAnimator.SetBool("isClose",true);
                defaultSpeed += freeFallSpeed;
            }
        }

        if(!_isGrounded)
        {
            _isGrounded = Physics2D.Linecast(gameObject.transform.position,
                groundPoint.position, 1 << LayerMask.NameToLayer(groundLayerName));

            if(_isGrounded)
            {
                _gameController.eventCallbackController
                    .registerEvent(gameObject.name, lifeTime, false, onTimesUp);

                parachuteAnimator.enabled = false;
                _startFalling = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.ToLower().Equals("player"))
        {
            if (_onPickUp != null)
                _onPickUp(rewardType);

            destroy(gameObject);
        }
    }
}

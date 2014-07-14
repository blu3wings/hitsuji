using UnityEngine;
using System;
using System.Collections;

public class Goal : RewardBase 
{
    private Action<Reward.RewardContent> _onPickUp;
    private GameController _gameController;

    public override void initialize(GameController gameController, 
        Reward.RewardContent type, Action<Reward.RewardContent> onPickUp)
    {
        _onPickUp = onPickUp;
        _gameController = gameController;

        rewardType = type;

        _gameController.eventCallbackController
            .registerEvent(gameObject.name, lifeTime, false, onTimesUp);
    }

    private void onTimesUp(UnityEngine.Object obj)
    {
        try
        {
            _gameController.eventCallbackController
            .registerEvent(gameObject.name, disappearTime, false, destroy);
        }
        catch (Exception ex) { }        
    }

    public override void destroy(UnityEngine.Object obj)
    {
        _gameController.eventCallbackController.disposeEvent(gameObject.name);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.ToLower().Equals("player"))
        {
            if (_onPickUp != null)
                _onPickUp(rewardType);

            destroy(gameObject);
        }
    }
}
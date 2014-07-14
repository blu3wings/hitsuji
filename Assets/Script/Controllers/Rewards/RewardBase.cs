using UnityEngine;
using System;
using System.Collections;

public class RewardBase : MonoBehaviour 
{
    //This determine the duration of the reward will
    //stay before it starts to blink
    public float lifeTime;

    //This dictates the duration of the reward will
    //stay after the blinking starts
    public float disappearTime;

    public Reward.RewardContent rewardType;

    /// <summary>
    /// Collect will be used when the reward is collected.
    /// </summary>
    public virtual void collect() { }

    /// <summary>
    /// Remove the rewards from the scene
    /// </summary>
    public virtual void destroy(UnityEngine.Object obj) { }
    
    /// <summary>
    /// Initialize the reward object.
    /// </summary>
    /// <param name="gameController">The game controller object.</param>
    /// <param name="type">The reward type.</param>
    /// <param name="onPickUp">The callback when this reward is picked.</param>
    public virtual void initialize(GameController gameController, 
        Reward.RewardContent type,
        Action<Reward.RewardContent> onPickUp) { }
}

public class Reward
{
    public enum RewardContent
    {
        LIFE,
        SPEED_UP,
        NUKE,
        COIN,
        GIFT,
        SHIELD,
        TIME_EXTENSION,
        TIME_FREEZE,
        GOAL,
        NONE
    }
}

[Serializable]
public class RewardContent
{
    public Reward.RewardContent content;
    public int probabilityWeight;
}
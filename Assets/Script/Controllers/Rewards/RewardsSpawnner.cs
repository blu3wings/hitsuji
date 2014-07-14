using UnityEngine;
using System.Collections;

public class RewardsSpawnner : MonoBehaviour 
{
    public enum RewardType
    {
        CRATE,
        ENERGY_BALL,
        DYNAMITE
    }

    public Collider2D[] spawnBound;
    public string rootDirectory;
    public RewardType[] rewardType;
    public Reward.RewardContent[] rewardContent;
    public float frequency;
    public float multiplier;

    private UnityEngine.Object _storedObj;
    private GameController _gameController;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private int _counter = 0;
 
    public void initialize(int level, GameController gameController)
    {
        if(_storedObj == null)
            _storedObj = Resources.Load(
                rootDirectory + "/" + rewardType[0].ToString().ToLower());

        frequency = frequency - (level * multiplier);

        _gameController = gameController;
        _gameController.eventCallbackController
            .registerEvent(gameObject.name, frequency, true, onSpawn);

    }

    public void continueSpawn()
    {
        _gameController.eventCallbackController
            .registerEvent(gameObject.name, frequency, true, onSpawn);
    }

    public void cancelSpawn()
    {
        _gameController.eventCallbackController
            .cancelEventCallback(gameObject.name);
    }

    private void onSpawn(UnityEngine.Object obj)
    {
        if(spawnBound.Length > 1)
        {
            int id = UnityEngine.Random.Range(0, spawnBound.Length - 1);
            _minX = spawnBound[id].bounds.min.x;
            _minY = spawnBound[id].bounds.min.y;
            _maxX = spawnBound[id].bounds.max.x;
            _maxY = spawnBound[id].bounds.max.y;
        }
        else
        {
            _minX = spawnBound[0].bounds.min.x;
            _minY = spawnBound[0].bounds.min.y;
            _maxX = spawnBound[0].bounds.max.x;
            _maxY = spawnBound[0].bounds.max.y;
        }

        spawnReward();
    }

    private void spawnReward()
    {
        if (_storedObj == null) return;

        float x = UnityEngine.Random.Range(_minX,_maxX);
        float y = UnityEngine.Random.Range(_minY,_maxY);

        GameObject rewardObj = GameObject.Instantiate(_storedObj, 
            new Vector2(x, y), Quaternion.identity) as GameObject;

        if(rewardContent.Length > 1)
        {
            int id = UnityEngine.Random.Range(0, rewardContent.Length - 1);
            rewardObj.GetComponent<RewardBase>().initialize(
            _gameController, rewardContent[id], _gameController.onRewardPlayer);

        }
        else
        {
            rewardObj.GetComponent<RewardBase>().initialize(
            _gameController, rewardContent[0], _gameController.onRewardPlayer);
        }

        

        rewardObj.name = rewardType[0].ToString() + "_" + _counter;
        _counter++;
    }
}
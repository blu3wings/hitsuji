using UnityEngine;
using System.Collections;

public class EnemySpawnner : MonoBehaviour 
{
    public enum EnemyType
    {
        BIRD,
        KAMIKAZEBIRD,
        FISH,
        WOLF,
        BEAR,
        KANGAROO,
        RAM,
        ROADRUNNER,
        RACOON,
        DRAGON,
        GHOST,
        TURTLE
    }

    public enum Direction
    {
        LEFTTORIGHT,
        RIGHTTOLEFT
    }

    public string rootDirectory;
    public EnemyType[] type; //The enemy type
    public Direction direction;
    public float frequency = 5.0f;
    public float multiplier = 0.03f;
    private Object _storedObj;
    private GameController _gameController;

    private int _counter = 0;

    public void initialize(int level,GameController gameController)
    {
        if(_storedObj == null)
            _storedObj = Resources.Load(
                rootDirectory + "/" + type[0].ToString().ToLower());

        frequency = frequency - (level * multiplier);

        _gameController = gameController;
        _gameController.eventCallbackController
            .registerEvent(gameObject.name, frequency, true, onSpawn);
        spawnEnemy(); //Always spawn the leading asset.
    }

    public void continueSpawn()
    {
        _gameController.eventCallbackController
            .registerEvent(gameObject.name, frequency, true, onSpawn);

        spawnEnemy();
    }

    public void cancelSpawn()
    {
        _gameController.eventCallbackController
            .cancelEventCallback(gameObject.name);
    }

    private void onSpawn(UnityEngine.Object obj)
    {
        spawnEnemy();
    }

    public void spawnEnemy()
    {
        //Debug.Log(rootDirectory + "/" + type[0].ToString().ToLower());   

        if (_storedObj == null) return;

        GameObject enemyGo = GameObject.Instantiate(_storedObj,
            transform.position, Quaternion.identity) as GameObject;

        _counter++;
        enemyGo.name = type[0].ToString().ToLower() + _counter.ToString();
        enemyGo.GetComponent<AIBehavior>().registerBehavior(_gameController);
        if (direction == Direction.LEFTTORIGHT)
            enemyGo.GetComponent<AIBehavior>().changeDirection(false);
        else
            enemyGo.GetComponent<AIBehavior>().changeDirection(true);

        _gameController.ai.Add(enemyGo.GetComponent<AIBehavior>());
    }
}
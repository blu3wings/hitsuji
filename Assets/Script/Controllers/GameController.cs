using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySampleAssets.CrossPlatformInput;

public class GameController : MonoBehaviour
{
    public const float DEFAULT_TIMER_VALUE = 120;

    #region Public Variables
    public GameObject playerCharacter;
	public UIController uiController;
    public TimerController timerController;
    public ExperimentalPixels.CameraController cameraController;
    public CharacterController playerController;
    public EventCallback eventCallbackController;
    public GameObject stopperPrefab;
    public EnemySpawnner[] spawnnerObj;
    public RewardsSpawnner[] rewardSpawnner;
    public Vector2 cameraMaxXandY;
    public Vector2 cameraMinXandY;
    public bool isSpawnEnemy = false;
    public bool isSpawnReward = false;

    #endregion Public Variables

    #region Private Variables

    private int _level = 1;
    private int _lives = 5;
    private GameObject _playerObj;

    #endregion Private Variables

    #region Properties

    private GameObject _startStopper;
    public GameObject startStopper
    {
        get { return _startStopper; }
    }

    private GameObject _endStopper;
    public GameObject endStopper
    {
        get { return _endStopper; }
    }

    private List<AIBehavior> _ai = new List<AIBehavior>();
    public List<AIBehavior> ai
    {
        get { return _ai; }
        set { _ai = value; }
    }

    #endregion Properties

    #region 1 - Setup

    private void setup()
    {
        setupStoppers(); //Setup stoppers to prevent the character from moving beyond the camera view.

        uiController.updateLabelText(UILabelName.UI_LIVES, _lives.ToString());

        if (isSpawnEnemy)
            spawnEnemy();

        if (isSpawnReward)
            spawnReward();

        setupTimer();
    }

    private void setupTimer()
    {
        timerController.initialize(DEFAULT_TIMER_VALUE, onTimesUp);
    }

    private void setupStoppers()
    {
        //Get the four corners of the camera's viewport
        Vector3[] camFourCorners = cameraController.gameCamera.GetWorldCorners();

        //Calculate the width and height using the four corners
        float width = camFourCorners[2].x - camFourCorners[1].x;
        float height = camFourCorners[1].y - camFourCorners[0].y;

        //Create start stopper
        if (_startStopper == null)
            _startStopper = GameObject.Instantiate(stopperPrefab) as GameObject;

        _startStopper.name = "StartStopper";
        _startStopper.transform.position = new Vector2(cameraMinXandY.x - (width / 2), 0);

        //Create end stopper
        if (_endStopper == null)
            _endStopper = GameObject.Instantiate(stopperPrefab) as GameObject;

        _endStopper.name = "EndStopper";
        _endStopper.transform.position = new Vector2(cameraMaxXandY.x + (width / 2), 0);
    }

    #endregion 1 - Setup

    #region 2 - Spawn

    private void spawnPlayer()
    {
        if (_playerObj != null)
            Destroy(_playerObj);

        _playerObj = GameObject.Instantiate(playerCharacter,
           Vector3.zero, Quaternion.identity) as GameObject;
        _playerObj.name = "player";
        _playerObj.GetComponent<CharacterController_Touch>().initialize();

        cameraController.switchBehavior("ExperimentalPixels.Follow", _playerObj);
    }

    private void spawnReward()
    {
        foreach(RewardsSpawnner rs in rewardSpawnner)
        {
            rs.initialize(_level, this);
        }
    }

    private void spawnEnemy()
    {
        foreach (EnemySpawnner e in spawnnerObj)
        {
            e.initialize(_level, this);
        }
    }

    #endregion 2 - Spawn

    #region 3 - Callback Functions

    private void onTimesUp()
    {
        if (_playerObj != null)
            _playerObj.GetComponent<CharacterController_Touch>().dead();
    }

    public void onRewardPlayer(Reward.RewardContent content)
    {
        Debug.Log("Rewarding player with " + content);
    }

    private void onPlayerDied(UnityEngine.Object obj)
    {
        _lives--;

        if (_lives < 1)
        {
            uiController.showGameOverPanel(true);
        }
        else
        {
            uiController.showIntermissionPanel(true);
            uiController.updateLabelText(UILabelName.UI_INTERMISSION_LIVES, _lives.ToString());
            uiController.updateLabelText(UILabelName.UI_LIVES, _lives.ToString());
        }
    }

    private void onContinue()
    {      
        resetLevel();

        uiController.showIntermissionPanel(false);
        uiController.showGameStatsPanel(true);
    }

    private void onGameOver()
    {
        resetLevel();

        _lives = 5;

        uiController.showGameOverPanel(false);
        uiController.updateLabelText(UILabelName.UI_LIVES, _lives.ToString());
        uiController.showGameStatsPanel(true);
    }

    public void playerDied()
    {
        foreach (EnemySpawnner e in spawnnerObj)
        {
            if(e != null)
                e.cancelSpawn();
        }

        foreach (AIBehavior aiBehavior in _ai)
        {
            if(aiBehavior != null)
                aiBehavior.freeze();
        }

        //Hold for 4 seconds before showing the Intermission panel
        eventCallbackController.registerEvent(gameObject.name, 4, false, onPlayerDied);

        timerController.stop();
    }

    #endregion 3 - Callback Functions

    #region 4 - Reset Functions

    private void destroyAllEnemyObj()
    {
        //First destroy all enemy objects
        foreach (AIBehavior ai in _ai)
        {
            if (ai != null)
                Destroy(ai.gameObject);
        }
    }

    private void destroyPlayerObj()
    {
        if (_playerObj != null)
            Destroy(_playerObj);
    }

    private void resetCamera()
    {
        Camera.main.transform.position
            = new Vector3(0, Camera.main.transform.position.y,
                Camera.main.transform.position.z);
    }

    private void resetLevel()
    {
        //Reset all the enemy object
        destroyAllEnemyObj();

        //Reset the camera
        resetCamera();

        //Re-spawn the player obj
        spawnPlayer();

        //Re-spawn the enemy
        spawnEnemy();

        //Reset timer
        timerController.initialize(DEFAULT_TIMER_VALUE, onTimesUp);
    }

    #endregion 4 - Reset Functions

    private void Start()
    {
        cameraController.connectGameController(this);
        //spawnPlayer();
        //uiController.registerUICallbacks(UIAction.INTERMISSION_CONTINUE, onContinue);
        //uiController.registerUICallbacks(UIAction.BUTTON_PLAYAGAIN, onGameOver);
        //setup();
    } 
    
    public void onMainMenuClicked()
    {
        
    }
}
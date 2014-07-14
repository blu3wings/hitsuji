using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExperimentalPixels
{
    public class CameraController : MonoBehaviour
    {
        public CameraBase[] behaviors;
        public Vector2 cameraMaxXandY;
        public Vector2 cameraMinXandY;
        private CameraBase _currentBehavior;

        private GameController _gameController;
        public GameController gameController
        {
            get { return _gameController; }
        }

        private Camera _gameCamera;
        public Camera gameCamera
        {
            get { return _gameCamera; }
        }

        private Dictionary<string, CameraBase> _registeredBehavior = new Dictionary<string, CameraBase>();

        public void connectGameController(Object obj)
        {
            _gameController = (GameController)obj;
            cameraMaxXandY = _gameController.cameraMaxXandY;
            cameraMinXandY = _gameController.cameraMinXandY;

            if (gameObject.GetComponent<Camera>())
                _gameCamera = gameObject.GetComponent<Camera>();

            foreach (CameraBase behavior in behaviors)
            {
                behavior.registerBehavior(this, onBehaviorRegistered);
            }
        }

        private void onBehaviorRegistered(CameraBase behaviorObj, string name)
        {
            Debug.Log(string.Format("[{0}] {1} is registered.", GetType().FullName, name));

            if (!_registeredBehavior.ContainsKey(name))
            {
                _registeredBehavior.Add(name, behaviorObj);
            }

            behaviorObj.enabled = false;
        }

        public void setTarget(GameObject target)
        {
            if (_currentBehavior != null)
                _currentBehavior.target = target.transform;
            else
                Debug.Log(string.Format("[{0}] Current behavior is null. " +
                    "Unable to set target", GetType().FullName));
        }

        public void switchBehavior(string behaviorName, GameObject target)
        {
            switchBehavior(behaviorName);
            setTarget(target);
        }

        public void switchBehavior(string behaviorName)
        {
            if (_currentBehavior != null)
                _currentBehavior.enabled = false;

            if (_registeredBehavior.ContainsKey(behaviorName))
            {
                _currentBehavior = _registeredBehavior[behaviorName];
                _currentBehavior.enabled = true;
            }
        }
    }
}
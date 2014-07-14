using UnityEngine;
using System;
using System.Collections;

namespace ExperimentalPixels
{
    public class Follow : CameraBase
    {
        public float xMargin = 1f;
        public float yMargin = 1f;
        public float xSmooth = 8f;
        public float ySmooth = 8f;

        private void FixedUpdate()
        {
            trackPlayer();
        }

        private void trackPlayer()
        {
            float targetX = transform.position.x;
            float targetY = transform.position.y;

            if (checkXMargin())
            {
                targetX = Mathf.Lerp(transform.position.x, target.position.x, xSmooth * Time.deltaTime);
            }

            if (checkYMargin())
            {
                targetY = Mathf.Lerp(transform.position.y, target.position.y, ySmooth * Time.deltaTime);
            }

            targetX = Mathf.Clamp(targetX,
                controller.cameraMinXandY.x, controller.cameraMaxXandY.x);
            targetY = Mathf.Clamp(targetY,
                controller.cameraMinXandY.y, controller.cameraMaxXandY.y);

            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }

        private bool checkXMargin()
        {
            return Mathf.Abs(transform.position.x - target.position.x) > xMargin;
        }

        private bool checkYMargin()
        {
            return Mathf.Abs(transform.position.y - target.position.y) > yMargin;
        }

        public override void registerBehavior(UnityEngine.Object controllerObj,
            Action<CameraBase, string> callback)
        {
            controller = (CameraController)controllerObj;

            if (callback != null)
                callback(this, GetType().FullName);
        }
    }
}
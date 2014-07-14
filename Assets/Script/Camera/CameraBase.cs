using UnityEngine;
using System;
using System.Collections;

namespace ExperimentalPixels
{
    public class CameraBase : MonoBehaviour
    {
        public Transform target;
        public CameraController controller;

        public virtual void registerBehavior(UnityEngine.Object obj,
            Action<CameraBase, string> callback) { }
    }
}
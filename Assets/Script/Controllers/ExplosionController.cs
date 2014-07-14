using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour 
{
    public GameObject parent;

    public void destroyObject()
    {
        if (parent != null)
            Destroy(parent);
        else
            Destroy(gameObject);
    }
}
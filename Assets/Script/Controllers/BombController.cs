using UnityEngine;
using System;
using System.Collections;

public class BombController : MonoBehaviour 
{
    public GameObject explosionPrefab;
    public Transform explosionPoint;
    public string[] objectToInteract;

    private Action _onHit;

    public void registerObjToController(Action callback)
    {
        _onHit = callback;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (string tagName in objectToInteract)
        {
            if (col.gameObject.tag.ToLower().Contains(tagName))
            {
                if (_onHit != null)
                    _onHit();

                if (explosionPrefab != null)
                {
                    GameObject explosionObj = GameObject.Instantiate(explosionPrefab,
                        explosionPoint.position, Quaternion.identity) as GameObject;
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (string tagName in objectToInteract)
        {
            if (col.gameObject.tag.ToLower().Contains(tagName))
            {
                if (col.gameObject.tag.ToLower().Equals("player"))
                {
                    if (!col.gameObject.GetComponent<CharacterController_Touch>().isDead)
                            col.gameObject.GetComponent<CharacterController_Touch>().dead();
                }

                if (_onHit != null)
                    _onHit();

                if (explosionPrefab != null)
                {
                    GameObject explosionObj = GameObject.Instantiate(explosionPrefab,
                        explosionPoint.position, Quaternion.identity) as GameObject;
                }

                Destroy(gameObject);
            }
        }
    }
}
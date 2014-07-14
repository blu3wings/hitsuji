using UnityEngine;
using System.Collections;

public class BomberBird : BirdAI 
{
    public GameObject bombPrefab;

    private bool _bombAllowed = true;

    public void Start()
    {
        //Only if the player object is still available. Player object may get destroy
        //while the bird is being instantiated.
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player");

        initializeAI();
    }

    public override void playerInRange()
    {
        if (!_bombAllowed) return;

        Vector2 spawnPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 3f);

        //When player is in range, release the bomb
        GameObject bombObj = GameObject.Instantiate(
            bombPrefab, spawnPos, 
            Quaternion.Euler(bombPrefab.transform.rotation.eulerAngles)) as GameObject;

        bombObj.GetComponent<BombController>()
                .registerObjToController(onBombardment);

        _bombAllowed = false;
    }

    private void onBombardment()
    {
        //Reset and instatiate a new bomb for the next volley.
        _bombAllowed = true;
    }
}
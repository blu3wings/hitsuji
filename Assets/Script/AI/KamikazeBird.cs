using UnityEngine;
using System.Collections;

public class KamikazeBird : BirdAI
{
    public string[] objectToInteract;
    public GameObject featherPrefab;
    public int numberOfFeathers;
    public float forceToFeather;
    public GameObject smackPrefab;
    public Transform smackPoint;

    public void Start()
    {
        initializeAI();
    }

    public override void playerInRange()
    {
        if (isDiving) return;

        isDiving = true;

        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(facingDirection == -1)
            transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
        else
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        birdEyeAnimator.SetBool("isDive", true);
        birdAnimator.SetBool("isDive", true);

        changeTarget();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (string tagName in objectToInteract)
        {
            if (col.gameObject.tag.ToLower().Contains(tagName))
            {
                if(col.gameObject.tag.ToLower().Equals("player"))
                {
                    if (!col.gameObject.GetComponent<CharacterController_Touch>().isDead)
                            col.gameObject.GetComponent<CharacterController_Touch>().dead();
                }

                if (smackPrefab != null)
                {
                    GameObject explosionObj = GameObject.Instantiate(smackPrefab,
                        smackPoint.position, Quaternion.identity) as GameObject;
                }

                for (int i = 0; i < numberOfFeathers; i++ )
                {
                    float xPos = UnityEngine.Random.Range(-1f, 1f);
                    float yPos = UnityEngine.Random.Range(1f, 2f);
                    int direction = 1;

                    if (xPos >= 0 && yPos >= 1.5f)
                        direction = 1;
                    else
                        direction = -1;


                    Vector2 newPos = new Vector2(smackPoint.position.x + xPos,
                        smackPoint.position.y + yPos);

                    GameObject featherObj = GameObject.Instantiate(featherPrefab,
                        newPos, Quaternion.identity) as GameObject;
                    featherObj.transform.localScale = new Vector3(direction, 1, 1);

                    featherObj.rigidbody2D.AddForce(Vector2.up * forceToFeather);
                }

                Destroy(gameObject);
            }
        }
    }
}
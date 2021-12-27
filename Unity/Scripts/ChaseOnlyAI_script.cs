using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseOnlyAI_script : MonoBehaviour
{
    public float speed = 2;
    private Transform playerLocation;
    private GameObject player;
    private bool collisionBounce = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLocation = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(collisionBounce)
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerLocation.position.x,playerLocation.position.y), -(speed * 2) * Time.deltaTime);
        else
        transform.position = Vector2.MoveTowards(transform.position, playerLocation.position, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log(collision.gameObject.name);
        
        if (collision.gameObject.tag == "Player")
        {
            Player_Stats_Script player = collision.gameObject.GetComponent<Player_Stats_Script>();
            //Debug.Log("Player hit");
            player.GetHurt(10);
            //  Debug.Log(player.health);
            StartCoroutine(CollisionBounce());
        }
    }

    public void BounceAway()
    {
        StartCoroutine(CollisionBounce());
    }
        
    
    IEnumerator CollisionBounce()
    {
        collisionBounce = true;
        yield return new WaitForSeconds(.25f);
        collisionBounce = false;
    }
}

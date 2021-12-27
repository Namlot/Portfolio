using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int damage = 15;
    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
      //  gameObject.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void WakeUP()
    {
        gameObject.SetActive(true);
        
        StartCoroutine(Attack());

    }
    
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f,1f);
        gameObject.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f,.5f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player_Stats_Script player = collision.gameObject.GetComponent<Player_Stats_Script>();
            //Debug.Log("Player hit");
            player.GetHurt(damage);
            //  Debug.Log(player.health);
            gameObject.GetComponent<Collider2D>().enabled = false;
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }
}

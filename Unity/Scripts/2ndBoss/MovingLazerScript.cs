using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLazerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = .0625f;
    private int damage = 20;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        StartCoroutine(DeathTimer());
    }
    void FixedUpdate()
    {
        transform.Translate(0, speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        Player_Stats_Script player = collision.gameObject.GetComponent<Player_Stats_Script>();
        if (player != null)
        {
            player.GetHurt(damage);
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private int speed = 10;
    public int damage = 5;

    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
                break;
            case "wall":
            case "tilemap":
                Destroy(this.gameObject);
                break;
            case "Player":
                collision.gameObject.GetComponent<Player_Stats_Script>().GetHurt(damage);
                Destroy(this.gameObject);
                break;
        }
    }
}

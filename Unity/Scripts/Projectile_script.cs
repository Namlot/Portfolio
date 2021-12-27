using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_script : MonoBehaviour
{
    private int speed = 10;
    public float damage = 1;
    private float timeToDie = 1;
    private void Start()
    {
        damage += GameObject.Find("Player").GetComponent<Player_Stats_Script>().attackModifier;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("Player").GetComponent<Collider2D>());
        StartCoroutine(DeathTimer());
    }
    void Update()
    {
        transform.Translate(0, speed  * Time.deltaTime,0 );
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                collision.gameObject.GetComponent<Enemy_Stats_Script>().GetHurt(damage);
                Destroy(this.gameObject);
               
                break;
            case "wall":
            case "tilemap":
                Destroy(this.gameObject);
                break;
            case "Player":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
                break;
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlash_Script : MonoBehaviour
{
    private int speed = 10;
    public int damage = 1;

    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }
}

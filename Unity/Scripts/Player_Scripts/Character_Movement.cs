using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
 //   public float MAXSPEED = 5f; //FIXME: Make Constant at some point. Keeping this here, probably won't use.
    public float SPEEDINCREMENT = 6F; //FIXME: Make Constant when satisfied 

    Rigidbody2D rb2d;

    private void Start()
    {
       
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;

    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        Vector2 speedToAdd = new Vector2(0, 0);
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            speedToAdd.y += SPEEDINCREMENT;
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            speedToAdd.y += -SPEEDINCREMENT;
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            speedToAdd.x += SPEEDINCREMENT;
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            speedToAdd.x += -SPEEDINCREMENT;
        }
       
        rb2d.velocity = speedToAdd;
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dash_Script : MonoBehaviour
{
    Rigidbody2D rb2d;
    private bool canDash = true;
    public int dashSpeed = 3;
    private void Start()
    {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
      

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentSpeed = rb2d.velocity;
        
        if (Input.GetKey("space") && canDash)
        {
            GameObject.Find("DodgeSound").GetComponent<AudioSource>().Play();
            canDash = false;
            gameObject.GetComponent<Character_Movement>().enabled = false;
            
            rb2d.velocity *= dashSpeed;
           // Debug.Log(rb2d.velocity);
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(.1f);
        
        gameObject.GetComponent<Character_Movement>().enabled = true;

        yield return new WaitForSeconds(1f);
        canDash = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Script : MonoBehaviour
{

    private Transform playerLocation;
    private GameObject player;
    public float speed = 10f;
    private bool didCollide = false;
    bool canAttack = true;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLocation = player.GetComponent<Transform>();
        Physics2D.IgnoreLayerCollision(8,8);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 5f && canAttack)
        {
            canAttack = false;
            StartChargeAttack();
        }

    }
    void StartChargeAttack()
    {
        didCollide = false;
        gameObject.GetComponent<ChaseOnlyAI_script>().enabled = false;
        StartCoroutine(StopRotation());
        StartCoroutine(ChargeAttack(1.25f, 50));
    }

    IEnumerator ChargeAttack(float WaitTime, float Timeleft)
    {
        yield return new WaitForSeconds(WaitTime);
        // transform.position += transform.right * Time.deltaTime * speed;
      
        if (Timeleft > 0 && !didCollide)
        {
            StartCoroutine(ChargeAttack(.01f, --Timeleft));
        }
        else
        {
            didCollide = false;
            StartCoroutine(WaitBetweenAttacks());
            gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
        }
    }

    IEnumerator WaitBetweenAttacks()
    {
        
        yield return new WaitForSeconds(2f);
        canAttack = true;
        gameObject.GetComponent<ChaseOnlyAI_script>().enabled = true;
        gameObject.GetComponent<RotateTowardsPlayer>().enabled = true;
        
    }

    public void StartWait()
    {
        StartCoroutine(WaitBetweenAttacks());
    }
    IEnumerator StopRotation()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<RotateTowardsPlayer>().enabled = false;
        
       gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right * speed);

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("big boss collided with" + other.gameObject.name);
        if (other.gameObject.tag == "wall" || other.gameObject.tag == "Player")
        {
            didCollide = true;
        }
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Stats_Script>().GetHurt(10);
            
        }
    }

    
   


}

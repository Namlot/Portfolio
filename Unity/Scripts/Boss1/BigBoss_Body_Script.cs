using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoss_Body_Script : MonoBehaviour
{

    private Transform playerLocation;
    public GameObject player;
    public GameObject bossTurret;
    private int attackOption = 0;
    private int lastAttack = 0;
    private bool didCollide = false;
    private bool willRotate = false;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerLocation = player.GetComponent<Transform>();
        StartNextAttack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (willRotate)
        {
            Vector3 pos = player.transform.position;
            Vector3 dir = pos - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 99);
        }
    }

    void AttackOne()
    {
        didCollide = false;
        willRotate = true;
        StartCoroutine(StopRotation());
        StartCoroutine(ChargeAttack(1.25f, 50));
    }
   
    IEnumerator ChargeAttack(float WaitTime, float Timeleft)
    {
        yield return new WaitForSeconds(WaitTime);
        transform.position += transform.right * Time.deltaTime * speed;
        if (Timeleft > 0 && !didCollide)
        {
            StartCoroutine(ChargeAttack(.01f, --Timeleft));
        }
        else
        {
            didCollide = false;
            attackOption = -1;
            StartCoroutine(WaitBetweenAttacks());
        }
    }

    IEnumerator WaitBetweenAttacks()
    {
        if (gameObject.GetComponent<Enemy_Stats_Script>().health < 25)
            yield return new WaitForSeconds(.5f);
        else
            yield return new WaitForSeconds(2f);
        
        StartNextAttack();
    }

   public void StartWait()
    {
        StartCoroutine(WaitBetweenAttacks());
    }

    IEnumerator StopRotation()
    {
        yield return new WaitForSeconds(1f);
        willRotate = false;
        GameObject.Find("RevSound").GetComponent<AudioSource>().Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("big boss collided with" + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "wall":
                didCollide = true;
                break;
            case "Player":
                didCollide = true;
                other.gameObject.GetComponent<Player_Stats_Script>().GetHurt(20);
                break;
        }
    }

    void StartNextAttack()
    {
        //Randomly Choose Attack (But not last attack used)
        if (lastAttack == 0)
            attackOption = Random.Range(1, 3);
        else if (lastAttack == 2)
            attackOption = Random.Range(0, 2);
        else
        {
          int randomNum = Random.Range(0, 2);
          if (randomNum == 0)
              attackOption = 0;
          else
              attackOption = 2;
        }         
        lastAttack = attackOption;
       

        //Launch Attacks
        if (attackOption == 0)
        {
            attackOption = -1;
            AttackOne();
        }
        else if (attackOption == 1)
        {
            attackOption = -1;
            bossTurret.GetComponent<BossTurret_Script>().Attack2();
        }
        else if (attackOption == 2)
        {
            attackOption = -1;
            bossTurret.GetComponent<BossTurret_Script>().Attack3();
        }
    }
}

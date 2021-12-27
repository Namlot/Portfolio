using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject projectilePrefab;
    public GameObject projectile;
   // private float reloadTime = 50f;
    private GameObject BigBoss;
    
    // Start is called before the first frame update
    void Start()
    {
       // Attack2();
        BigBoss = GameObject.Find("BigBoss");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
   

    public void Attack2()
    {
        GameObject.Find("MachineGun").GetComponent<AudioSource>().Play();
        gameObject.GetComponent<RotateTowardsPlayer>().enabled = false;
        StartCoroutine(BulletStorm(.25f, 350));
    }

    IEnumerator BulletStorm(float timeTowait, float TimeRemaining)
    {
        yield return new WaitForSeconds(timeTowait);
        int randomNum = Random.Range(5, 10);
        transform.Rotate(0, 0, randomNum);
        
        if(TimeRemaining % 3 == 1)
        Fire();
        if  (TimeRemaining > 0)
        StartCoroutine(BulletStorm(.01f, --TimeRemaining));
        else
        {
            gameObject.GetComponent<RotateTowardsPlayer>().enabled = true;
            BigBoss.GetComponent<BigBoss_Body_Script>().StartWait();
            GameObject.Find("MachineGun").GetComponent<AudioSource>().Stop();
        }
    }


    public void Attack3()
    {
        GameObject.Find("MachineGun").GetComponent<AudioSource>().Play();
        StartCoroutine(TargetedAttack(0f, 25));
    }
    IEnumerator TargetedAttack(float waitTime, int timeRemaining)
    {
        
        yield return new WaitForSeconds(waitTime);
        
        int randomNum = Random.Range(-10, 10);
        transform.Rotate(0, 0, randomNum);
       
        Fire();
       
        if (timeRemaining > 0)
            StartCoroutine(TargetedAttack(.2f, --timeRemaining));
        else
        {
            GameObject.Find("MachineGun").GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<RotateTowardsPlayer>().enabled = true;
            BigBoss.GetComponent<BigBoss_Body_Script>().StartWait();
        }
    }


    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }



    void Fire() {
        
            
            projectile = Instantiate(projectilePrefab) as GameObject;
            projectile.transform.position = transform.TransformPoint(0, 0, 0);
            projectile.transform.rotation = transform.rotation;
            projectile.transform.Rotate(0, 0, 270);
           
            // projectile.transform.rotation
            //  Debug.Log(projectile.transform.rotation);
        
    }
}

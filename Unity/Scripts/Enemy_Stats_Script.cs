using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Stats_Script : MonoBehaviour
{
    public float health;
    public float maxHP;
    public int soulWorth;
    public bool boss;
    Color baseColor;
    void Start()
    {
        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        maxHP = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHurt(float damageTaken)
    {
        StartCoroutine(ColorChange());
        health -= damageTaken;
        if(health > 0)
        {
            GameObject.Find("BulletHit").GetComponent<AudioSource>().Play();
        }
        else 
        {
            if(gameObject.GetComponent<SmileBossScript>() != null)
            {
                GameObject.Find("DeathLaugh").GetComponent<AudioSource>().Play();
            }
            else
            GameObject.Find("EnemyDeath").GetComponent<AudioSource>().Play();

            GameObject.Find("Player").GetComponent<Player_Stats_Script>().SetSouls(soulWorth);
            if (boss) GameObject.Find("HUD").GetComponent<UpdateHUD>().Win();
            Destroy(gameObject);
        }
    }

    IEnumerator ColorChange()
    {   
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;
    }
}

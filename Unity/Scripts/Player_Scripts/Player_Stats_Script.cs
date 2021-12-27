using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Stats_Script : MonoBehaviour
{
    public int maxMana = 50;
    public int maxHP = 100;
    private int time = 0;
    public int health = 100;
    public int souls = 0;
    public int mana = 50;
    public Text healthText, soulText, timerText, manaText;
    private bool isInvincible = false;
    public GameObject gameOver;
    public float attackModifier = 0;
    GameObject[] enemyList;
    // Start is called before the first frame update
    void Start()
    {
        soulText = GameObject.Find("SoulText").GetComponent<Text>();
        timerText = GameObject.Find("TimeText").GetComponent<Text>();

        StartCoroutine(TimeUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        soulText.text = "Souls: " + souls;

        if (Input.GetKeyDown("j"))
            isInvincible = isInvincible ^ true;
        if (Input.GetKey("escape"))
            Application.Quit();
        if (Input.GetKeyDown("r"))
            Application.LoadLevel(Application.loadedLevel);
    }

    public void GetHurt(int damageTaken)
    {
        if (!isInvincible)
        {
            enemyList = GameObject.FindGameObjectsWithTag("enemy");
            foreach(GameObject g in enemyList)
            {
              //  Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

            }
            GameObject.Find("GetHurt").GetComponent<AudioSource>().Play();
            isInvincible = true;
            health -= damageTaken;
            
            if (health <= 0)
            {
                GameObject.Find("HUD").GetComponent<UpdateHUD>().Lose();
                gameObject.GetComponent<Character_Movement>().SPEEDINCREMENT = 0;
                GameObject.Find("Gun").GetComponent<Gun_Script>().canFire = false;
            }
            else
                StartCoroutine(InvincibilityFrames(1));
        }
    }

    IEnumerator InvincibilityFrames(int timeRemaining)
    {   
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("Gun").GetComponent < SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.2f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Gun").GetComponent < SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.2f);
        if (timeRemaining > 0)
            StartCoroutine(InvincibilityFrames(--timeRemaining));
        else
        {
            enemyList = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject g in enemyList)
            {
              //  Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(),false);

            }
            isInvincible = false;
        }
    }
    
    public void SetSouls(int numSouls)
    {
        souls += numSouls;
    }

    IEnumerator TimeUpdate()
    {
        yield return new WaitForSeconds(1f);
        time++;
        timerText.text = "Time: " + time;
        if (mana < maxMana)
            mana++;
        StartCoroutine(TimeUpdate());
    }
}

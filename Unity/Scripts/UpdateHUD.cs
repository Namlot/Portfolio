using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This class updates the HUD according to whatever values are assigned (e.g. player health/mana correspond to HUD health/mana bars)


public class UpdateHUD : MonoBehaviour
{
    public GameObject hp;
    public GameObject mana;
    public GameObject bossHp;
    public GameObject staff;
    public GameObject fire;
    public GameObject shield;
    public GameObject player;
    public GameObject boss;
    public GameObject victory;
    public GameObject gameover;
    const float BOSS_WIDTH = 160;
    const float BOSS_HEIGHT = 30;
    const float MAX_WIDTH = 67.4f;
    const float HEIGHT = 21;
    const float MIN_DIFF = 0.5f;
    const float REDUCE_DIFF = 10;

    // Start is called before the first frame update
    void Start()
    {
      //  if (staff.GetComponent<StaffScript>().spell == "fire")
            fire.SetActive(true);
       // else
         //   shield.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float diff = 0;
        //player hp/mana
        Player_Stats_Script stats = player.GetComponent<Player_Stats_Script>();
        //smoothly update player hp
        //if we need to make a change (i.e. width != health / maxHP * maxWidth)
        if (Math.Abs(diff = MAX_WIDTH * (float)stats.health / (float)stats.maxHP - hp.GetComponent<RectTransform>().sizeDelta.x) != 0)
        {
            //use a variable rate (faster when there's more of a difference remaining, smaller when less)
            if (Math.Abs(diff) > MIN_DIFF)
                diff /= REDUCE_DIFF;
            hp.GetComponent<RectTransform>().sizeDelta = new Vector2(hp.GetComponent<RectTransform>().sizeDelta.x + diff, HEIGHT);
        }

        //repeat for mana
        if (Math.Abs(diff = MAX_WIDTH * (float)stats.mana / (float)stats.maxMana - mana.GetComponent<RectTransform>().sizeDelta.x) != 0)
        {
            if (Math.Abs(diff) > MIN_DIFF)
                diff /= REDUCE_DIFF;
            mana.GetComponent<RectTransform>().sizeDelta = new Vector2(mana.GetComponent<RectTransform>().sizeDelta.x + diff, HEIGHT);
        }
        //if the boss has spawned
        if (boss != null && boss.activeSelf)
        {
            //repeat for the boss, too
            Enemy_Stats_Script bossStats = boss.GetComponent<Enemy_Stats_Script>();

            //repeat for mana
            if (Math.Abs(diff = BOSS_WIDTH * (float)bossStats.health / (float)bossStats.maxHP - bossHp.GetComponent<RectTransform>().sizeDelta.x) != 0)
            {
                if (Math.Abs(diff) > MIN_DIFF)
                    diff /= REDUCE_DIFF;
                bossHp.GetComponent<RectTransform>().sizeDelta = new Vector2(bossHp.GetComponent<RectTransform>().sizeDelta.x + diff, BOSS_HEIGHT);
            }
        }

        /* Insert spell icon cooldown animation here */
        //if (staff.GetComponent<StaffScript>().CurrentStaff == "")
    }

    public void SwapSpells()
    {
        //toggle visibility on both icons (whichever is active turns inactive and vice versa)
        fire.SetActive(fire.activeSelf ^ true);
        shield.SetActive(shield.activeSelf ^ true);
    }

    public void Win()
    {
        victory.SetActive(true);
        GameObject.Find("Boss HP").SetActive(false);
        StartCoroutine(Restart());
    }

    public void Lose()
    {
        gameover.SetActive(true);
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Prototype");
    }

}

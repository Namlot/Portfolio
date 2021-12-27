using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject bossHUD;
    public GameObject[] bosses;
    static System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        //pick the boss we're spawning
        boss = bosses[rand.Next(0, bosses.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 50)
        {
            boss.SetActive(true);
            if (boss == bosses[1])
                boss = GameObject.Find("SmileBoss");
            GameObject.Find("HUD").GetComponent<UpdateHUD>().boss = boss;
            bossHUD.SetActive(true);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffScript : MonoBehaviour
{
    public GameObject spellCast;
    public GameObject fireCirclePrefab;
    public GameObject shieldPrefab;
    public GameObject player;
    public float Range;
    public int manaCost;
    public string spell;
    public bool ready = true;

    void Start()
    {
        spell = "fire";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Player_Stats_Script stats = player.GetComponent<Player_Stats_Script>();
        //if they can't use the spell or they aren't trying to, return now
        if (!ready || !Input.GetMouseButtonDown(1) || stats.mana < manaCost)
            return;
        switch (spell)
        {
            case "fire":
                
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.position) < Range)
                {
                    ready = false;
                    stats.mana -= manaCost;
                    spellCast = Instantiate(fireCirclePrefab) as GameObject;
                    spellCast.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -8f);
                    StartCoroutine(Cooldown(1.5f));
                }
                break;
            case "shield":
                ready = false;
                spellCast = Instantiate(shieldPrefab) as GameObject;
                spellCast.transform.position = transform.TransformPoint(-.75f, 0, -8);
                spellCast.transform.rotation = transform.rotation;
                stats.mana -= manaCost;
                StartCoroutine(Cooldown(4f));
                break;
            default:
                Debug.Log("Error - Attempting to cast a spell that doesn't exist.");
                Debug.Log(spell);
                break;
        }
    }

    IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ready = true;
    }

    public void SwapSpells()
    {
        if (spell == "fire")
        {
            spell = "shield";
            manaCost = 15;
        }
        else
        {
            spell = "fire";
            manaCost = 10;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject merchantText;
    public GameObject player;
    public int cost;
    public string itemType;
    
    void Start()
    {
        merchantText = gameObject.transform.Find("MerchantText").gameObject;
        merchantText.SetActive(false);
        player = GameObject.Find("Player");
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        Player_Stats_Script stats = player.GetComponent<Player_Stats_Script>();
        if (Input.GetKeyDown("e") && stats.souls >= cost && merchantText.activeSelf)
        {
            stats.souls -= cost;
            GameObject.Find("ChaChingSound").GetComponent<AudioSource>().Play();
            switch (itemType)
            {
                case "rapidfire":
                    GameObject.Find("Gun").GetComponent<Gun_Script>().SetAttackSpeed(-.2f);
                    Destroy(gameObject);
                    break;
                case "mightyglacier":
                    stats.attackModifier += 1;
                    player.GetComponent<Character_Movement>().SPEEDINCREMENT *= .5f;
                    player.GetComponent<Player_Dash_Script>().dashSpeed *= 2;
                    Destroy(gameObject); break;
                case "healthmerchant":
                    if (stats.health + 10 >= stats.maxHP)
                        stats.health = stats.maxHP;
                    else
                        stats.health += 10;
                    Destroy(gameObject); break;
                case "staff":
                    StaffScript staff = GameObject.Find("Staff").GetComponent<StaffScript>();
                    staff.SwapSpells();
                    GameObject.Find("HUD").GetComponent<UpdateHUD>().SwapSpells();
                    Setup(); //update the text
                    break;
            }
        }
        merchantText.SetActive(Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < 1);
    }
    
    void Setup()
    {
        TextMesh description = merchantText.GetComponent<TextMesh>();
        switch (itemType)
        {
            case "rapidfire":
                cost = 10;
                description.text = "RapidFire (10 Souls)  [E]";
                break;
            case "mightyglacier":
                cost = 0;
                description.text = "Mighty Glacier (Curse)  [E] \n Movement DOWN Attack UP";
                break;
            case "healthmerchant":
                cost = 3;
                description.text = "+10 HP (3 Souls)  [E]";
                break;
            case "staff":
                StaffScript staff = GameObject.Find("Staff").GetComponent<StaffScript>();
                if (staff.spell == "fire")
                    description.text = "Switch to Shield Staff  [E]";
                else
                    description.text = "Switch to Fire Staff  [E]";
                cost = 0;
                break;
            default:
                description.text = "";
                cost = 0;
                break;
        }
    }
}

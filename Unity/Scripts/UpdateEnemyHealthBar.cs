using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject healthBar;
    public GameObject healthScale;
    public float startingHealth;
    void Start()
    {
        healthBar.SetActive(true);
        healthScale.SetActive(true);
        startingHealth = gameObject.GetComponent<Enemy_Stats_Script>().health;
    }

    // Update is called once per frame
    void Update()
    {
        healthScale.transform.localScale = new Vector3((gameObject.GetComponent<Enemy_Stats_Script>().health) / startingHealth, 1, 1);
    }
}

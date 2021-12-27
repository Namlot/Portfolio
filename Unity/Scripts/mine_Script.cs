using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mine_Script : MonoBehaviour
{
    public GameObject projectile;
    public GameObject enemyBulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("MineExplosion").GetComponent<AudioSource>().Play();
        for (int i = 0; i < 8; ++i)
        {
            projectile = Instantiate(enemyBulletPrefab) as GameObject;
            projectile.transform.position = transform.TransformPoint(0, 0, 0);
            projectile.transform.rotation = transform.rotation;
            projectile.transform.Rotate(0, 0, -45 * i);
        }
        Destroy(gameObject);
    }
}

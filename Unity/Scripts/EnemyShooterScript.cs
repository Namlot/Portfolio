using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectile;
    private float reloadTime = 70f;
    private bool canFire = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cooldown());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canFire && (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < 10 || gameObject.GetComponent<Renderer>().isVisible))
        {
            GameObject.Find("EnemyFire").GetComponent<AudioSource>().Play();
            canFire = false;
            projectile = Instantiate(projectilePrefab) as GameObject;
            projectile.transform.position = transform.TransformPoint(0, 0, 0);
            projectile.transform.rotation = transform.rotation;
            projectile.transform.Rotate(0, 0, 270);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(reloadTime * Time.deltaTime);
        canFire = true;
    }

}

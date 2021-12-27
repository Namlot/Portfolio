using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Script : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectile;
    private float reloadTime = .65f;
    public bool canFire = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && canFire) 
        {
            canFire = false;
            projectile = Instantiate(projectilePrefab) as GameObject;
            projectile.transform.position = transform.TransformPoint(0,.1f,0);
            projectile.transform.rotation = transform.rotation;

            //Play Audio
            GameObject.Find("GunShot").GetComponent<AudioSource>().Play();


            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(reloadTime);
        canFire = true;
    }

    public void SetAttackSpeed(float amountToChange)
    {
        reloadTime += amountToChange;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
     
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;
        Vector3 dir = pos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 99);


        //transform.right = player.transform.position - transform.position;
        //transform.rotation =new Quaternion(0, 0, transform.rotation.z, 0);
        // transform.LookAt(new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyRader : MonoBehaviour
{
    public GameObject player;
    public int detectionRange;
    public GameObject objectToSpawn;
    public GameObject spawnedObject;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < detectionRange && gameObject.GetComponent<Renderer>().isVisible)
        {
          //  Debug.Log("Player Sighted");
            spawnedObject = Instantiate(objectToSpawn) as GameObject;
            spawnedObject.transform.position = transform.TransformPoint(0, 0, 0);
            spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y, 0);
            Destroy(this.gameObject);
        }

    }
    //private void OnRenderObject()
    //{
    //        Debug.Log("Player Sighted");
    //        spawnedObject =  Instantiate(objectToSpawn) as GameObject;
    //        spawnedObject.transform.position = transform.TransformPoint(0, 0, 0);
    //       spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y, 0);
    //        Destroy(this.gameObject);
    //}

}

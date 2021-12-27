using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLayer_Script : MonoBehaviour
{
    public GameObject mine;
    public GameObject minePrefab;
    // Start is called before the first frame update
    public int speed;
    void Start()
    {
        
        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        StartCoroutine(LayMine());
        //Pick a direction to start
        float xDirection = Random.Range(-100, 100) * 0.01f;
        float yDirection = Random.Range(-100, 100) * 0.01f;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xDirection,yDirection) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = speed * (gameObject.GetComponent<Rigidbody2D>().velocity.normalized);
    }
    
    IEnumerator LayMine()
    {
       yield return new WaitForSeconds(5f);
       mine = Instantiate(minePrefab) as GameObject;
       mine.transform.position = gameObject.transform.position;
       StartCoroutine(LayMine());
    }
}

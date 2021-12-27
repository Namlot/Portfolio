using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    public GameObject enterText;
    public GameObject player;
    public Vector3Int target;

    // Start is called before the first frame update
    void Start()
    {
        enterText.SetActive(false);
        player = GameObject.Find("Player");
        //enterText.transform.position = g*ameObject.transform.position + new Vector3(1.05f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && enterText.activeSelf)
            player.transform.position = target;

        enterText.SetActive(Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < 2);
    }
}

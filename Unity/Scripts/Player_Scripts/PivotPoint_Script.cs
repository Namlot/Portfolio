using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPoint_Script : MonoBehaviour
{
    public bool willRotate = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      //  transform.position = GameObject.Find("Player").transform.position;
        if (willRotate)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 99 * Time.deltaTime);
        }
    }
}

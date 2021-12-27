using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireCircle : MonoBehaviour
{
    public float width;

    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(DeathTimer());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    IEnumerator ColliderActivateDeactivate()
    {
       
        
    
         RaycastHit2D[] hitResults =  Physics2D.CircleCastAll(gameObject.transform.position, width, new Vector2(0, 0));
        foreach(RaycastHit2D hit in hitResults)
        {
            if (hit.collider.gameObject.tag == "enemy")
            {
                hit.collider.gameObject.GetComponent<Enemy_Stats_Script>().GetHurt(1);
            }
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ColliderActivateDeactivate());
    }
    IEnumerator DeathTimer()
    {
        StartCoroutine(ColliderActivateDeactivate());
        yield return new WaitForSeconds(1.25f);
       
         Destroy(gameObject);
    }
    
}

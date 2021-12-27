using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileBossScript : MonoBehaviour
{
    public GameObject teleportPadSouth, teleportPadNorth, teleportPadEast, teleportPadWest;
    int currentTeleportPosition = 0;
    
    public GameObject[] verticalLazers;
    public GameObject[] horizontalLazers;
    public GameObject[] horizontalMovingLazers;
    public GameObject currentMovingLazer;
    public GameObject EvilFireCircle;
    public GameObject EvilFireCirclePrefab;
    public GameObject Player;
    public int lastAttack;
    public int attackCount = 0;
    private float verticalScaling = -.5f;
    public bool debugAttacks = false;
    private Vector2 velocity;
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = teleportPadSouth.transform.position;
        verticalLazers = GameObject.FindGameObjectsWithTag("VerticalLazer");
        horizontalLazers = GameObject.FindGameObjectsWithTag("HorizontalLazer");
        foreach (GameObject G in verticalLazers)
        {
            G.SetActive(false);
        }
        foreach (GameObject G in horizontalLazers)
        {
            G.SetActive(false);
        }
        StartCoroutine(ReadyNextAttack(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (debugAttacks)
        {
            debugAttacks = false;
            ChooseAttack();
        }
    }
 
    void ChooseAttack()
    {
        
        if (attackCount == 2)
        {
            GameObject.Find("ClownLaugh1").GetComponent<AudioSource>().Play();
            Teleport();
            attackCount = 0;
        }
        else
        {
            attackCount++;

            int newAttack = lastAttack;
            while (newAttack == lastAttack)
                newAttack = Random.Range(0, 3);

            lastAttack = newAttack;
            if(lastAttack == 0)
            {
                GameObject.Find("ClownLaugh2").GetComponent<AudioSource>().Play();
                StartMovingLazerAttack();
            }
            else if(lastAttack == 1)
            {
                GameObject.Find("ClownLaugh3").GetComponent<AudioSource>().Play();
                StartCoroutine(FireCircleAttack());
            }
            else
            {
                GameObject.Find("ClownLaugh4").GetComponent<AudioSource>().Play();
                LazerAttack();
            }

        }
    }
    void Teleport()
    {
        int choice = currentTeleportPosition;
        while(choice == currentTeleportPosition)
        choice = Random.Range(0, 4);

        //Debug.Log(choice);
        currentTeleportPosition = choice;

        switch (choice)
        {
            case 0:
                gameObject.transform.position = teleportPadSouth.transform.position;
                gameObject.transform.rotation = teleportPadSouth.transform.rotation;
                break;
            case 1:
                gameObject.transform.position = teleportPadEast.transform.position;
                gameObject.transform.rotation = teleportPadEast.transform.rotation;
                break;
            case 2:
                gameObject.transform.position = teleportPadWest.transform.position;
                gameObject.transform.rotation = teleportPadWest.transform.rotation;
                break;
            default:
                gameObject.transform.position = teleportPadNorth.transform.position;
                gameObject.transform.rotation = teleportPadNorth.transform.rotation;
                break;
        }
        StartCoroutine(ReadyNextAttack(1.5f));
    }

    void LazerAttack()
    {
        if (Random.Range(0, 2) == 1)
            StartCoroutine(SequenceAttack(horizontalLazers));
        else
            StartCoroutine(SequenceAttack(verticalLazers));
    }
    IEnumerator SequenceAttack(GameObject[] lazerArray)
    {
        int lazerChoice = Random.Range(0, 2);
        for (int i = lazerChoice; i < lazerArray.Length; i += 2)
        {
            lazerArray[i].GetComponent<LazerScript>().WakeUP();
        }
        yield return new WaitForSeconds(2.5f);
        if (lazerChoice == 0)
            lazerChoice = 1;
        else
            lazerChoice = 0;
        for (int i = lazerChoice; i < lazerArray.Length; i += 2)
        {
            lazerArray[i].GetComponent<LazerScript>().WakeUP();
        }
        StartCoroutine(GridAttack());
    }
    IEnumerator GridAttack()
    {
        yield return new WaitForSeconds(2.6f);
        int lazerChoice = Random.Range(0, 2);
        for (int i = lazerChoice; i < verticalLazers.Length; i += 2)
        {
            verticalLazers[i].GetComponent<LazerScript>().WakeUP();
        }
       
        if (lazerChoice == 0)
            lazerChoice = 1;
        else
            lazerChoice = 0;

        for (int i = lazerChoice; i < horizontalLazers.Length; i += 2)
        {
            horizontalLazers[i].GetComponent<LazerScript>().WakeUP();
        }

        StartCoroutine(ReadyNextAttack(3f));

    }
    IEnumerator FireCircleAttack(int timeRemaining = 10)
    {
        EvilFireCircle = Instantiate(EvilFireCirclePrefab) as GameObject;
       
        EvilFireCircle.transform.position = Player.transform.position - new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        
        EvilFireCircle.GetComponent<LazerScript>().WakeUP();
        yield return new WaitForSeconds(.5f);
        if (timeRemaining > 0)
            StartCoroutine(FireCircleAttack(--timeRemaining));
        else
            StartCoroutine(ReadyNextAttack(2f));
    }

    void StartMovingLazerAttack()
    {
        switch (currentTeleportPosition)
        {
            case 0:
                StartCoroutine(MovingLazerAttack(horizontalMovingLazers, teleportPadSouth));
                break;
            case 1:
                StartCoroutine(MovingLazerAttack(horizontalMovingLazers, teleportPadEast));
                break;
            case 2:
                StartCoroutine(MovingLazerAttack(horizontalMovingLazers, teleportPadWest));
                break;
            case 3:
                StartCoroutine(MovingLazerAttack(horizontalMovingLazers, teleportPadNorth));
                break;
        }
    }

    IEnumerator MovingLazerAttack(GameObject[] lazerArray, GameObject teleportPad)
    {
        ArrayShuffle(lazerArray);

        for (int i = 0; i < lazerArray.Length; i++)
        {
            currentMovingLazer = Instantiate(lazerArray[i]) as GameObject;
            currentMovingLazer.transform.position = teleportPad.transform.position;
            currentMovingLazer.transform.rotation = teleportPad.transform.rotation;
            if (currentTeleportPosition == 1 || currentTeleportPosition == 2)
                currentMovingLazer.transform.localScale += new Vector3(verticalScaling, 0, 0);
            yield return new WaitForSeconds(2f);
        }

        StartCoroutine(ReadyNextAttack(3f));
    }

    private void ArrayShuffle(GameObject[] gameObjectArray)
    {
        for (var i = gameObjectArray.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            GameObject tmp = gameObjectArray[i];
            gameObjectArray[i] = gameObjectArray[r];
            gameObjectArray[r] = tmp;
        }
    }

    IEnumerator ReadyNextAttack(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        ChooseAttack();
    }
}

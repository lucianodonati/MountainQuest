using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement_Coordinator : MonoBehaviour {

    public Enemy_Movement[] movements;
    public Enemy_Movement currentMovement;

    //CYCLE OF MOVEMENT
    //The first entry in the array is always the first behaviour done on spawning
    //the second is the next
    //and so forth until it hits the end and loops around.
    //KEEP IN MIND: This is for non-seek movements.
    public string[] cycleOfMovement;
    private int currHOMindex;

    private float changeTimer;
    public float changeTimerMax = 5.0f;
    // adds randomness into the timer mix;
    public float changeTimerError = 0.5f;

	// Use this for initialization
	void Start () {
        movements = gameObject.GetComponents<Enemy_Movement>();

        for (int i = 0; i < movements.Length; ++i)
        {
            movements[i].enabled = false;
            if (movements[i].GetType() == System.Type.GetType(cycleOfMovement[currHOMindex]))
            {
                currentMovement = movements[i];
                currentMovement.enabled = true;
            }
        }

        changeTimer = changeTimerMax;
	}
	
	// Update is called once per frame
	void Update () {
        if(currentMovement.GetType() != System.Type.GetType("Seek_Movement"))
        {
            changeTimer -= Time.deltaTime;

            if(changeTimer <= 0.0f)
            {
                currHOMindex++;

                if (currHOMindex >= cycleOfMovement.Length)
                    currHOMindex = 0;

                for (int i = 0; i < movements.Length; ++i)
                {
                    if (movements[i].GetType() == System.Type.GetType(cycleOfMovement[currHOMindex]))
                    {
                        currentMovement.enabled = false;
                        currentMovement = movements[i];
                        currentMovement.enabled = true;
                        break;
                    }
                }

                changeTimer = changeTimerMax + Random.Range(-changeTimerError, changeTimerError);
            }
        }
        else
        {
            if(((Seek_Movement)currentMovement).aggroTimer <= 0.0f)
            {
                currHOMindex = 0;

                for(int i = 0; i < movements.Length; ++i)
                {
                    if(movements[i].GetType() == System.Type.GetType(cycleOfMovement[currHOMindex]))
                    {
                        currentMovement.enabled = false;
                        currentMovement = movements[i];
                        currentMovement.enabled = true;
                        break;
                    }
                }
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        for(int i = 0; i < movements.Length; ++i)
        {
            if(movements[i].GetType() == System.Type.GetType("Seek_Movement"))
            {
                if (((Seek_Movement)movements[i]).InFOV(GameObject.FindGameObjectWithTag("Player")))
                {
                    currentMovement.enabled = false;
                    currentMovement = movements[i];
                    currentMovement.enabled = true;
                    break;
                }
            }
        }
    }
}

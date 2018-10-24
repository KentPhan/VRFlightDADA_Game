using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour {
    public List<GameObject> listOfCheckpoints = new List<GameObject>();

    private int currentCheckpointIndex = 0;

	// Use this for initialization
	void Start () {
        setFirstCheckpoint();
	}
	
	void setFirstCheckpoint()
    {
        listOfCheckpoints[currentCheckpointIndex].SetActive(true);
    }

    public void setNextCheckpoint()
    {
        //Deactivate the current checkpoint 
        listOfCheckpoints[currentCheckpointIndex].SetActive(false);
        //check if current index is the last checkpoint
        if (currentCheckpointIndex == listOfCheckpoints.Count - 1)
        {
            //If it was the last checkpoint.. go to win screen or whatever
            allCheckpointsReached();

        }
        else
        {
            //Increase the index
            currentCheckpointIndex++;


            //Activate the next checkpoint

            listOfCheckpoints[currentCheckpointIndex].SetActive(true);
        }

    }

    void allCheckpointsReached()
    {
        //for now just logging in debug window
        Debug.Log("I guess u reached the end");
    }

}

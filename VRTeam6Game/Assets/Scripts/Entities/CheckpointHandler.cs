using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointHandler : MonoBehaviour {
    public List<GameObject> listOfCheckpoints = new List<GameObject>();
   
    public Image fadeInImage;
    public GameObject player;

    private int currentCheckpointIndex = 0;
    private Vector3 playerStartPosition;
	// Use this for initialization
	void Start () {
        setFirstCheckpoint();
        playerStartPosition = player.transform.position;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            resetToLastCheckpoint();
        }
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

    public void resetToLastCheckpoint()
    {
        setScreenFade();
       
        setPlayerPosition();

    }

    void setScreenFade()
    {
        var tempcolor = fadeInImage.color;
        tempcolor.a = 0.01f;
        fadeInImage.color = tempcolor;
        
        fadeInImage.CrossFadeAlpha(255f, 1f, false);
        Invoke("stopScreenFade", 1f);
    }
    void stopScreenFade()
    {
        fadeInImage.CrossFadeAlpha(0f, 1f, false);
    }

    void setPlayerPosition()
    {
        if (currentCheckpointIndex != 0)
            player.transform.GetComponent<Assets.Scripts.Entities.PlayerEntity>().setPosition(listOfCheckpoints[currentCheckpointIndex].transform.localPosition);
        else
            player.transform.GetComponent<Assets.Scripts.Entities.PlayerEntity>().setPosition(playerStartPosition);


    }

}

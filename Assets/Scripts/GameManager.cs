using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int timeLimit;
    public int currentTime;

	// Use this for initialization
	void Start () {
        //Planning stuff?		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentTime++;
            print("Next Timestep");
        }
	}
}

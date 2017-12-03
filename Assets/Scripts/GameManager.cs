using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Additional notes
//Every item has its own id
//Every locationn has its own id
//Picking up and dropping off refer to locations??

public class GameManager : MonoBehaviour {

    public int timeLimit;
    public int currentTime;

    public AIState currentState;
    public List<AIState> observedStates;

    private Planner planner = new Planner();

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            planner.Search(currentState);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTime++;
            print("Next Timestep");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTime--;
            print("Previous Timestep");
        }
	}
}
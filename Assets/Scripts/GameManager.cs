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

    public List<Ingredient> availableIngredients;
    public List<Pot> pots;
    public List<Plane> plates;

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
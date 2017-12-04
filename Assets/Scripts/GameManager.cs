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

    void Start()
    {
        currentState = new AIState();

        BoardState board = new BoardState(0);
        IngredientState onion = new IngredientState(1, IngredientType.ONION, false, false, false);
        MealState meal = new MealState(3, false, new List<int>());
        PotState pot = new PotState(2, meal.ID);

        currentState.ItemStateList = new List<ItemState>() {board,
                                                            onion,
                                                            pot,
                                                            meal};

        currentState.CurrentTableState = new TableState(0);
        currentState.CurrentPlayerState = new PlayerState();

        currentState.BoardStateIndexList = new List<int> { 0 };
        currentState.IngredientStateIndexList = new List<int>() { 1 };
        currentState.PotStateIndexList = new List<int>() { 2 };
        currentState.MealStateIndexList = new List<int>() { 3 };
        currentState.PlateStateIndexList = new List<int>() { };

        planner.goal = new CookGoal();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            planner.Search(currentState);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
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
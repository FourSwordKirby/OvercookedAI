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

    public ItemManager IM;
    private Planner planner = new Planner();

    public List<AIState> observedStates;

    public List<Action> currentPlan;
    public int currentPlanIndex;

    void Start()
    {
        //currentState = new AIState();

        //BoardState board = new BoardState(0);
        //IngredientState onion = new IngredientState(1, IngredientType.ONION, false, false, false);
        //MealState meal = new MealState(3, false, new List<int>());
        //PotState pot = new PotState(2, meal.ID);
        //TableSpace table = new TableSpace(4);

        //currentState.ItemStateList = new List<ItemState>() {board,
        //                                                    onion,
        //                                                    pot,
        //                                                    meal,
        //                                                    table};
        
        //currentState.CurrentPlayerState = new PlayerState();

        //currentState.BoardStateIndexList = new List<int> { 0 };
        //currentState.IngredientStateIndexList = new List<int>() { 1 };
        //currentState.PotStateIndexList = new List<int>() { 2 };
        //currentState.MealStateIndexList = new List<int>() { 3 };
        //currentState.PlateStateIndexList = new List<int>() { };
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AIState currentState = IM.GetWorldState();
            planner.goal = new CookGoal();

            currentPlan = planner.Search(currentState);
            currentPlanIndex = 0;

            Debug.Log(currentPlan.Count);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTime++;
            print("Next Timestep");

            if(currentPlanIndex < currentPlan.Count)
            {
                AIState CurrentState = ApplyAction(currentPlan[currentPlanIndex]);
                if (observedStates.Count < currentTime)
                    observedStates.Add(CurrentState);
                else
                    observedStates[currentTime] = CurrentState;
                IM.LoadWorldState(CurrentState);
                Debug.Log("Action applied. History size: " + observedStates.Count);

                currentPlanIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTime--;
            print("Previous Timestep");

            IM.LoadWorldState(observedStates[currentTime]);
            currentPlanIndex++;
        }
	}

    private AIState ApplyAction(Action a)
    {
        AIState CurrentState = IM.GetWorldState();

        if (!a.isValid(CurrentState))
        {
            Debug.Log("Action is not valid. Ignoring.");
            return null;
        }

        CurrentState = a.ApplyAction(CurrentState);
        return CurrentState;
    }
}
﻿using System.Collections;
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

    public Player PlayerRef;
    public GameObject HighlightedObject;
    public GameObject Highlighter;
    public GameObject HighlightPrefab;
    public Action HighlightedAction;
    public int HighlightedIndex;
    public AIState CurrentState;

    private void Awake()
    {
        Highlighter = Instantiate(HighlightPrefab, transform);
    }

    void Start()
    {
        IM = FindObjectOfType<ItemManager>();
        PlayerRef = FindObjectOfType<Player>();
        StartCoroutine(LateStart());
    }


    IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate();
        CurrentState = IM.GetWorldState();
        HighlightedIndex = 0;
        NextHighlight();
    }

    private void PrevHighlight()
    {
        HighlightedObject = GetHighlight(-1);
        Highlighter.transform.position = new Vector3(
            HighlightedObject.transform.position.x,
            Highlighter.transform.position.y,
            HighlightedObject.transform.position.z);
    }

    private void NextHighlight()
    {
        HighlightedObject = GetHighlight(1);
        Highlighter.transform.position = new Vector3(
            HighlightedObject.transform.position.x,
            Highlighter.transform.position.y,
            HighlightedObject.transform.position.z);
    }

    private GameObject GetHighlight(int step)
    {
        HighlightedIndex += step;
        if (HighlightedIndex >= IM.ItemList.Count)
        {
            HighlightedIndex = -IM.IngredientSpawners.Count;
        }
        else if (HighlightedIndex < -IM.IngredientSpawners.Count)
        {
            HighlightedIndex = IM.ItemList.Count - 1;
        }

        if (HighlightedIndex >= 0)
        {
            if (!PlayerRef.IsHolding)
            {
                PickUpAction puAction = new PickUpAction(HighlightedIndex);
                if (puAction.isValid(CurrentState))
                {
                    HighlightedAction = puAction;
                    return IM.ItemList[HighlightedIndex].gameObject;
                }
                else
                {
                    return GetHighlight(step);
                }
            }
            else
            {
                DropOffAction doAction = new DropOffAction(HighlightedIndex);
                if (doAction.isValid(CurrentState))
                {
                    HighlightedAction = doAction;
                    return IM.ItemList[HighlightedIndex].gameObject;
                }
                else
                {
                    return GetHighlight(step);
                }
            }
        }
        else
        {
            int spawnerIndex = ~HighlightedIndex;
            SpawnAction sAction = new SpawnAction(IM.IngredientSpawners[spawnerIndex].MyIngredientType);
            if (sAction.isValid(CurrentState))
            {
                HighlightedAction = sAction;
                return IM.IngredientSpawners[spawnerIndex].gameObject;
            }
            else
            {
                return GetHighlight(step);
            }
        }
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


        // Choose highlighted item
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextHighlight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevHighlight();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Applying action...");
            CurrentState = HighlightedAction.ApplyAction(CurrentState);
            IM.LoadWorldState(CurrentState);
            NextHighlight();
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
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

    public Heuristic CurrentHeuristic;
    public List<AIState> observedStates = new List<AIState>();

    public List<Action> currentPlan = new List<Action>();
    public int currentPlanIndex;

    public Player PlayerRef;
    public GameObject HighlightedObject;
    public GameObject Highlighter;
    public GameObject HighlightPrefab;
    public Action ZAction;
    public Action XAction;
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
        ZAction = null;
        XAction = null;

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
                PrepareAction prepAction = null;
                if (IM.ItemList[HighlightedIndex].MyItemType == ItemType.INGREDIENT)
                {
                    foreach (int boardID in IM.BoardIndexList)
                    {
                        Board b = IM.ItemList[boardID] as Board;
                        if (b.HoldingItem != null && b.HoldingItem.ID == HighlightedIndex)
                        {
                            prepAction = new PrepareAction(boardID);
                            XAction = prepAction;
                            break;
                        }
                    }
                }
                
                PickUpAction puAction = new PickUpAction(HighlightedIndex);
                if (puAction.isValid(CurrentState))
                {
                    ZAction = puAction;
                }
                else
                {
                    puAction = null;
                }

                if (puAction != null || prepAction != null)
                {
                    return IM.ItemList[HighlightedIndex].gameObject;
                }
                else
                {
                    return GetHighlight(step);
                }
            }
            else
            {
                SubmitOrderAction soAction = new SubmitOrderAction();
                if (soAction.isValid(CurrentState))
                {
                    XAction = soAction;
                }

                DropOffAction doAction = new DropOffAction(HighlightedIndex);
                if (doAction.isValid(CurrentState))
                {
                    ZAction = doAction;
                    return IM.ItemList[HighlightedIndex].gameObject;
                }

                TransferAction transAction = new TransferAction(HighlightedIndex);
                if (transAction.isValid(CurrentState))
                {
                    ZAction = transAction;
                    return IM.ItemList[HighlightedIndex].gameObject;
                }
                
                return GetHighlight(step);
            }
        }
        else
        {
            int spawnerIndex = ~HighlightedIndex;
            SpawnAction sAction = new SpawnAction(IM.IngredientSpawners[spawnerIndex].MyIngredientType);
            if (sAction.isValid(CurrentState))
            {
                ZAction = sAction;
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
            CurrentState = IM.GetWorldState();
            //planner.goal = new CookGoal();

            List<List<IngredientType>> goalRecipes = new List<List<IngredientType>>()
            {
                new List<IngredientType>() { IngredientType.ONION, IngredientType.ONION, IngredientType.MUSHROOM }
                //, new List<IngredientType>() { IngredientType.ONION, IngredientType.ONION, IngredientType.MUSHROOM  }
            };

            planner.goal = new FinishedMealGoal(goalRecipes);
            CurrentHeuristic = new IngredientBasedHeuristic(planner.goal as FinishedMealGoal);

            currentPlan = planner.Search(CurrentState);
            currentPlanIndex = 0;

            Debug.Log(currentPlan.Count);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTime++;
            print("Next Timestep");

            if(currentTime < observedStates.Count && currentPlanIndex != 0)
            {
                IM.LoadWorldState(observedStates[currentTime]);
            }
            else
            {
                if (currentPlanIndex < currentPlan.Count)
                {
                    ApplyAction(currentPlan[currentPlanIndex]);
                    currentPlanIndex++;
                }
                observedStates.Add(CurrentState);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentTime > 0)
            {
                currentTime--;
                print("Previous Timestep");

                IM.LoadWorldState(observedStates[currentTime]);
            }
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
            if (ZAction == null)
            {
                Debug.Log("There is no Z Action");
            }
            else
            {
                Debug.Log("Applying Z action...");
                ApplyAction(ZAction);
                NextHighlight();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (XAction == null)
            {
                Debug.Log("There is no X Action");
            }
            else
            {
                Debug.Log("Applying X action...");
                ApplyAction(XAction);
                NextHighlight();
            }
        }
    }

    private void ApplyAction(Action a)
    {
        Debug.Log("Applying action: " + a.ToString(CurrentState));
        if (!a.isValid(CurrentState))
        {
            Debug.Log("Action is not valid. Ignoring.");
            return;
        }

        CurrentState = a.ApplyAction(CurrentState);
        IM.LoadWorldState(CurrentState);
        Debug.Log("Action applied. History size: " + observedStates.Count);
        Debug.Log("Current state has heuristic: " + CurrentHeuristic.GetHeuristic(CurrentState));
    }
}
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
    private Planner planner;

    public FinishedMealGoal CurrentGoal;
    public Heuristic CurrentHeuristic;
    public List<AIState> observedStates = new List<AIState>();

    public List<Action> currentPlan = new List<Action>();
    public int currentPlanIndex;

    public Player PlayerRef;
    public GameObject PlayerStartLocation;
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
        PlayerStartLocation = new GameObject("Player Start Location");
        PlayerStartLocation.transform.position = PlayerRef.transform.position;
        CurrentGoal = new FinishedMealGoal(goalRecipes);
        CurrentHeuristic = new IngredientBasedHeuristic(CurrentGoal);
        Debug.Log("Start set the goal to be: " + CurrentGoal);
        StartCoroutine(LateStart());
    }


    IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate();
        CurrentState = IM.GetWorldState();
        Debug.Log(CurrentHeuristic + " = " + CurrentHeuristic.GetHeuristic(CurrentState));

        observedStates.Add(CurrentState);
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

    public OrderTrackerUI orderTracker;

    private bool playAutomatically;
    private float timeStep = 1.0f;
    private float counter = 0.0f;

    // Update is called once per frame
    void Update () {
        orderTracker.updateRecipes(goalRecipes);

        if(playAutomatically)
        {
            //Potentially do something with new recipes getting added in an replanning
            counter += Time.deltaTime;
            if(counter > timeStep)
            {
                counter = 0.0f;

                currentTime++;
                if (currentPlanIndex < currentPlan.Count)
                {
                    ApplyAction(currentPlan[currentPlanIndex]);
                    currentPlanIndex++;
                    observedStates.Add(CurrentState);
                }
                else
                    playAutomatically = false;
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            BeginSearch();
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

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetPlanner();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Expand10K();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SearchToCompletion();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            AIState temp = planner.PeekOpenSet();
            IM.LoadWorldState(temp);
            Debug.Log("Visualizing top of heap. " + CurrentHeuristic + " = " + CurrentHeuristic.GetHeuristic(temp));
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
        if(a is SpawnAction)
        {
            SpawnAction spawn = a as SpawnAction;
            IngredientSpawner spawner = IM.IngredientSpawners.Find(x => x.SpawnedIngredients.Exists(y => !y.IsSpawned && y.MyIngredientType == spawn.spawnType));
            HighlightedObject = spawner.gameObject;
        }
        if (a is PickUpAction)
        {
            PickUpAction pickup = a as PickUpAction;
            HighlightedObject = IM.ItemList[pickup.id].gameObject;
        }
        if (a is DropOffAction)
        {
            DropOffAction dropOff = a as DropOffAction;
            HighlightedObject = IM.ItemList[dropOff.id].gameObject;
        }
        if (a is TransferAction)
        {
            TransferAction transfer = a as TransferAction;
            HighlightedObject = IM.ItemList[transfer.id].gameObject;
        }
        if (a is PrepareAction)
        {
            PrepareAction prep = a as PrepareAction;
            HighlightedObject = IM.ItemList[prep.id].gameObject;
        }
        if (a is SubmitOrderAction)
        {
            HighlightedObject = IM.SubmittedTableRef.gameObject;
        }

        if (a is IdleAction)
        {
            HighlightedObject = PlayerStartLocation;
            Highlighter.transform.position = new Vector3(
                HighlightedObject.transform.position.x,
                Highlighter.transform.position.y,
                HighlightedObject.transform.position.z);
            PlayerRef.transform.position = Vector3.right * Highlighter.transform.position.x
                                            + Vector3.forward * Highlighter.transform.position.z;
        }
        else
        {
            Highlighter.transform.position = new Vector3(
                HighlightedObject.transform.position.x,
                Highlighter.transform.position.y,
                HighlightedObject.transform.position.z);
            PlayerRef.transform.position = Vector3.right * Highlighter.transform.position.x
                                            + Vector3.forward * (Highlighter.transform.position.z + 1);

        }


        Debug.Log("Applying action: " + a.ToString(CurrentState));
        if (!a.isValid(CurrentState))
        {
            Debug.LogError("Action is not valid. Ignoring.");
            return;
        }

        CurrentState = a.ApplyAction(CurrentState);
        IM.LoadWorldState(CurrentState);
        Debug.Log("Action applied. History size: " + observedStates.Count);
        Debug.Log(CurrentHeuristic + " = " + CurrentHeuristic.GetHeuristic(CurrentState));
    }

    //Used by the buttons
    public void ResetWorld()
    {
        if (observedStates.Count > 0)
        {
            CurrentState = observedStates[0].Clone() as AIState;
        }
        else
        {
            CurrentState = CurrentState.Clone() as AIState;
        }

        Debug.Log(CurrentHeuristic + " = " + CurrentHeuristic.GetHeuristic(CurrentState));
        IM.LoadWorldState(CurrentState);
        currentTime = 0;
        observedStates.Clear();
        currentPlan = null;
        observedStates.Add(CurrentState);
    }

    public void BeginSearch()
    {
        CurrentState = IM.GetWorldState();
        //planner.goal = new CookGoal();

        planner = new Planner(CurrentGoal, CurrentState, CurrentHeuristic);
        planner.SearchToCompletion();
        currentPlan = planner.Plan;
        currentPlanIndex = 0;

        //Clear out the observed states from the current time onwards
        observedStates.RemoveRange(currentTime, observedStates.Count - currentTime - 1);

        Debug.Log("Planner returned plan of length " + currentPlan.Count);
    }

    public void SetPlanner()
    {
        CurrentState = IM.GetWorldState();
        planner = new Planner(CurrentGoal, CurrentState, CurrentHeuristic);
        Debug.Log("Planner set.");
    }

    public void Expand10K()
    {
        planner.SearchLimited(10000);
        if(planner.IsFinished)
        {
            currentPlan = planner.Plan;
            currentPlanIndex = 0;

            //Clear out the observed states from the current time onwards
            observedStates.RemoveRange(currentTime, observedStates.Count - currentTime - 1);

            Debug.Log("Planner returned plan of length " + currentPlan.Count);
        }
    }

    public void SearchToCompletion()
    {
        planner.SearchToCompletion();
        if (planner.IsFinished)
        {
            currentPlan = planner.Plan;
            currentPlanIndex = 0;

            //Clear out the observed states from the current time onwards
            observedStates.RemoveRange(currentTime, observedStates.Count - currentTime - 1);

            Debug.Log("Planner returned plan of length " + currentPlan.Count);
        }
    }

    public static List<IngredientType> OnionSoup = new List<IngredientType>() { IngredientType.ONION, IngredientType.ONION, IngredientType.ONION };
    public static List<IngredientType> MushroomSoup = new List<IngredientType>() { IngredientType.MUSHROOM, IngredientType.MUSHROOM, IngredientType.MUSHROOM };

    List<List<IngredientType>> goalRecipes = new List<List<IngredientType>>()
        {
            OnionSoup
            //new List<IngredientType>() { IngredientType.ONION, IngredientType.ONION }
            //, new List<IngredientType>() { IngredientType.MUSHROOM }
        };

    public void AddOnionSoupOrder()
    {
        goalRecipes.Add(OnionSoup);
        CurrentGoal = new FinishedMealGoal(goalRecipes);
        CurrentHeuristic = new IngredientBasedHeuristic(CurrentGoal);
        Debug.Log("Goal updated to: " + CurrentGoal + ", Heuristic: " + CurrentHeuristic);
    }

    public void AddMushroomSoupOrder()
    {
        goalRecipes.Add(MushroomSoup);
        CurrentGoal = new FinishedMealGoal(goalRecipes);
        CurrentHeuristic = new IngredientBasedHeuristic(CurrentGoal);
        Debug.Log("Goal updated to: " + CurrentGoal + ", Heuristic: " + CurrentHeuristic);
    }

    public void Autoplay()
    {
        BeginSearch();
        playAutomatically = true;
    }

    public void StopAutoplay()
    {
        playAutomatically = false;
    }
}
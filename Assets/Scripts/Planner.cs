using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class Planner {


    /// <summary>
    /// The current goal of the planner, it is allowed to change during the course of the game?
    /// </summary>
    public Goal TargetGoal;
    public AIState StartState;
    public Heuristic HeuristicStrategy;
    public float Epsilon;
    public int Cost = 1;
    public List<Action> Plan;

    public bool IsFinished = false;
    public int NumberClosedStates = 0;
    public PriorityQueue<AIState> OpenSet = new PriorityQueue<AIState>();
    public Dictionary<AIState, AIState> AllStates = new Dictionary<AIState, AIState>(new AIStateComparator());
    public System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();

    public Planner(Goal goal, AIState startState, Heuristic h, float epsilon = 2f)
    {
        TargetGoal = goal;
        StartState = startState.Clone() as AIState;
        HeuristicStrategy = h;
        Epsilon = epsilon;

        StartState.GValue = 0;
        StartState.IsClosed = false;
        AllStates[StartState] = StartState;
        OpenSet.Enqueue(StartState, 0);
    }

    public bool ExpandState(AIState currentState)
    {
        currentState.IsClosed = true;
        ++NumberClosedStates;

        if (TargetGoal.IsGoal(currentState))
        {
            return true;
        }

        List<Action> validActions = GetValidActions(currentState);
        foreach (Action action in validActions)
        {
            AIState newState = action.ApplyAction(currentState);

            // If we have already seen this state, then pull out the existing reference to it.
            if (AllStates.ContainsKey(newState))
            {
                newState = AllStates[newState];
                if (newState.IsClosed)
                {
                    continue;
                }
            }
            else
            {
                AllStates[newState] = newState;
            }

            // Only insert into open list if the g-value is smaller
            int tentativeGValue = currentState.GValue + Cost;
            if (tentativeGValue < newState.GValue)
            {
                newState.GValue = tentativeGValue;
                newState.Parent = currentState;
                newState.ParentAction = action;

                float fValue = tentativeGValue + Epsilon * HeuristicStrategy.GetHeuristic(newState);
                OpenSet.Enqueue(newState, fValue);
            }
        }

        return false;
    }

    public void SearchToCompletion()
    {
        if (IsFinished)
        {
            Debug.LogWarning("Search has already completed. Create a new Planner to search a second time.");
            return;
        }

        System.Diagnostics.Stopwatch methodWatch = new System.Diagnostics.Stopwatch();

        Debug.Log("Searching to completion, current goal is: " + TargetGoal + ", heuristic is: " + HeuristicStrategy);
        methodWatch.Start();
        Watch.Start();
        while (OpenSet.Count > 0)
        {
            AIState currentState = OpenSet.Dequeue().Value;
            if(currentState.IsClosed)
            {
                continue;
            }

            bool done = ExpandState(currentState);
            if (done)
            {
                Watch.Stop();
                methodWatch.Stop();
                // Backtrack
                List<Action> plan = new List<Action>();
                while (currentState != StartState)
                {
                    plan.Add(currentState.ParentAction);
                    currentState = currentState.Parent;
                }

                plan.Reverse();
                Plan = plan;
                IsFinished = true;
                Debug.Log("Plan of size " + plan.Count + " found.");
                Debug.Log("Search completed in: " + (Watch.ElapsedMilliseconds / 1000f) + " sec");
                Debug.Log("Closed set: " + NumberClosedStates + " | Open set:" + (AllStates.Count - NumberClosedStates));
                return;
            }

            int timeoutSeconds = 60;
            if (methodWatch.ElapsedMilliseconds > timeoutSeconds * 1000)
            {
                Debug.LogError("Planner has taken over " + timeoutSeconds + " seconds. Aborting.");
                break;
            }
        }

        Watch.Stop();
        Debug.LogError("Plan not found");
        Debug.Log("Search completed in: " + (Watch.ElapsedMilliseconds / 1000f) + " sec");
        Debug.Log("Closed set: " + NumberClosedStates + " | Open set:" + (AllStates.Count - NumberClosedStates));
        return;
    }

    public void SearchLimited(int maxExpansion)
    {
        if (IsFinished)
        {
            Debug.LogWarning("Search has already completed. Create a new Planner to search a second time.");
            return;
        }

        Debug.Log("Expanding up to " + maxExpansion + " states... current goal is: " + TargetGoal + ", heuristic is: " + HeuristicStrategy);
        System.Diagnostics.Stopwatch methodWatch = new System.Diagnostics.Stopwatch();
        int currentExpansions = 0;

        methodWatch.Start();
        Watch.Start();
        while (OpenSet.Count > 0 && currentExpansions < maxExpansion)
        {
            AIState currentState = OpenSet.Dequeue().Value;
            if (currentState.IsClosed)
            {
                continue;
            }

            bool done = ExpandState(currentState);
            ++currentExpansions;
            if (done)
            {
                methodWatch.Stop();
                Watch.Stop();
                Debug.Log("Goal expanded after " + currentExpansions + " states were expanded.");

                // Backtrack
                List<Action> plan = new List<Action>();
                while (currentState != StartState)
                {
                    plan.Add(currentState.ParentAction);
                    currentState = currentState.Parent;
                }

                plan.Reverse();
                Plan = plan;
                IsFinished = true;
                Debug.Log("Plan of size " + plan.Count + " found.");
                Debug.Log("Method completed in: " + (methodWatch.ElapsedMilliseconds / 1000f) + " sec");
                Debug.Log("Full Search completed in: " + (Watch.ElapsedMilliseconds / 1000f) + " sec");
                Debug.Log("Closed set: " + NumberClosedStates + " | Open set:" + (AllStates.Count - NumberClosedStates));
                return;
            }
        }

        methodWatch.Stop();
        Watch.Stop();
        Debug.Log(maxExpansion + " expansions completed in " + (methodWatch.ElapsedMilliseconds / 1000f) + " sec");
        Debug.Log("Current total search time: " + (Watch.ElapsedMilliseconds / 1000f) + " sec");
        Debug.Log("Closed set: " + NumberClosedStates + " | Open set:" + (AllStates.Count - NumberClosedStates));
        return;
    }

    public int EvaluateCost(AIState state)
    {
        int[,] counts = state.GetMealIngredientCounts();
        foreach (List<int> goalRecipeCount in (TargetGoal as FinishedMealGoal).IngredientCountsPerRecipe)
        {
            foreach (int mealListIndex in state.PlateToMeal)
            {

            }
        }

        return Cost;
    }

    public AIState PeekOpenSet()
    {
        return OpenSet.Peek().Value;
    }

    /// <summary>
    /// Used to get all of the valid actions for the current state
    /// </summary>
    /// <returns></returns>
    public List<Action> GetValidActions(AIState state)
    {
        List<Action> validActions = new List<Action>();

        //Waiting around
        validActions.Add(new IdleAction());

        //Things you can do when your hands are free
        if(state.CurrentPlayerState.HandsFree())
        {
            //Spawning items
            foreach (IngredientType type in System.Enum.GetValues(typeof(IngredientType)))
            {
                SpawnAction spawnAction = new SpawnAction(type);
                if(spawnAction.isValid(state))
                    validActions.Add(spawnAction);
            }

            PickUpAction pickupAction;
            //Picking up everything
            //Ingredients
            foreach (int ingredientID in state.IngredientStateIndexList)
            {
                IngredientState ingredient = state.ItemStateList[ingredientID] as IngredientState;
                if(ingredient.IsSpawned && !ingredient.IsInMeal)
                {
                    pickupAction = new PickUpAction(ingredient.ID);
                    validActions.Add(pickupAction);
                }
            }

            //Pots
            foreach(int potID in state.PotStateIndexList)
            {
                PotState pot = state.ItemStateList[potID] as PotState;
                pickupAction = new PickUpAction(pot.ID);
                validActions.Add(pickupAction);
            }

            //Plates
            foreach (int plateID in state.PlateStateIndexList)
            {
                PlateState plate = state.ItemStateList[plateID] as PlateState;
                pickupAction = new PickUpAction(plate.ID);
                validActions.Add(pickupAction);
            }

            PrepareAction prepAction;
            foreach(int boardID in state.BoardStateIndexList)
            {
                BoardState bState = state.ItemStateList[boardID] as BoardState;
                if (!bState.IsFree())
                {
                    IngredientState iState = state.ItemStateList[bState.HoldingItemID] as IngredientState;
                    if(iState != null && !iState.IsPrepared)
                    {
                        prepAction = new PrepareAction(boardID);
                        validActions.Add(prepAction);
                    }
                }
            }
        }

        //Things you can do when you have something in hand
        else
        {
            DropOffAction dropoffAction;
            TransferAction transferAction;
            ItemState itemState = state.ItemStateList[state.CurrentPlayerState.HoldingItemID];
            ItemType type = itemState.MyItemType;

            if (type == ItemType.INGREDIENT)
            {
                //Putting things on the table
                foreach (int tableID in state.TableStateIndexList)
                {
                    TableSpace table = state.ItemStateList[tableID] as TableSpace;
                    if (table.IsFree())
                    {
                        dropoffAction = new DropOffAction(table.ID);
                        validActions.Add(dropoffAction);
                    }
                }

                //Moving ingredients to a cutting board
                foreach (int boardID in state.BoardStateIndexList)
                {
                    BoardState board = state.ItemStateList[boardID] as BoardState;
                    if(board.IsFree())
                    {
                        dropoffAction = new DropOffAction(board.ID);
                        validActions.Add(dropoffAction);
                    }
                }
                
                if((itemState as IngredientState).IsPrepared)
                {
                    //Moving ingredients to a pot
                    foreach (int potID in state.PotStateIndexList)
                    {
                        PotState pot = state.ItemStateList[potID] as PotState;
                        MealState meal = state.ItemStateList[pot.mealID] as MealState;
                        if (meal.MealSize() + 1 <= PotState.MAX_ITEMS_PER_POT && !meal.IsBurnt())
                        {
                            dropoffAction = new DropOffAction(pot.ID);
                            validActions.Add(dropoffAction);
                        }
                    }

                    //Moving ingredients to a plate
                    foreach (int plateID in state.PlateStateIndexList)
                    {
                        PlateState plate = state.ItemStateList[plateID] as PlateState;
                        MealState meal = state.ItemStateList[plate.mealID] as MealState;
                        if (!plate.IsSubmitted && !meal.IsBurnt())
                        {
                            dropoffAction = new DropOffAction(plate.ID);
                            validActions.Add(dropoffAction);
                        }
                    }
                }
            }

            if(type == ItemType.POT)
            {
                PotState pot = itemState as PotState;
                MealState meal = state.ItemStateList[pot.mealID] as MealState;

                //Putting the pot on the table
                foreach (int tableID in state.TableStateIndexList)
                {
                    TableSpace table = state.ItemStateList[tableID] as TableSpace;
                    if (table.IsFree())
                    {
                        dropoffAction = new DropOffAction(table.ID);
                        validActions.Add(dropoffAction);
                    }
                }

                //Moving meal to a pot
                transferAction = new TransferAction(Item.NOTHING_ID);
                foreach (int potID in state.PotStateIndexList)
                {
                    transferAction.id = potID;
                    if (transferAction.isValid(state))
                    {
                        validActions.Add(transferAction);
                        transferAction = new TransferAction(Item.NOTHING_ID);
                    }
                }

                //Moving the meal to another plate
                foreach (int plateID in state.PlateStateIndexList)
                {
                    transferAction.id = plateID;
                    if (transferAction.isValid(state))
                    {
                        validActions.Add(transferAction);
                        transferAction = new TransferAction(Item.NOTHING_ID);
                    }
                }
            }

            if (type == ItemType.PLATE)
            {
                PlateState plate = itemState as PlateState;

                //Putting things on the table
                foreach (int tableID in state.TableStateIndexList)
                {
                    TableSpace table = state.ItemStateList[tableID] as TableSpace;
                    if (table.IsFree())
                    {
                        dropoffAction = new DropOffAction(table.ID);
                        validActions.Add(dropoffAction);
                    }
                }

                //If the plate is non-empty
                MealState heldMeal = state.ItemStateList[plate.mealID] as MealState;
                if (heldMeal.IsSpawned())
                {

                    //Submitting the meal
                    if(heldMeal.IsCooked())
                    {
                        SubmitOrderAction submitAction = new SubmitOrderAction();
                        validActions.Add(submitAction);
                    }

                    //Moving meal to a pot
                    transferAction = new TransferAction(Item.NOTHING_ID);
                    foreach (int potID in state.PotStateIndexList)
                    {
                        transferAction.id = potID;
                        if (transferAction.isValid(state))
                        {
                            validActions.Add(transferAction);
                            transferAction = new TransferAction(Item.NOTHING_ID);
                        }
                    }

                    //Moving the meal to another plate
                    foreach (int plateID in state.PlateStateIndexList)
                    {
                        transferAction.id = plateID;
                        if (transferAction.isValid(state))
                        {
                            validActions.Add(transferAction);
                            transferAction = new TransferAction(Item.NOTHING_ID);
                        }
                    }
                }
            }
        }

        return validActions;
    }
    
}

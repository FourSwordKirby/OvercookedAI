using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class Planner {

    /// <summary>
    /// The current goal of the planner, it is allowed to change during the course of the game?
    /// </summary>
    public Goal goal;

    public ItemManager itemManager;

    public List<Action> Search(AIState startState)
    {
        Dictionary<AIState, List<Action>> closedStates = new Dictionary<AIState, List<Action>>();

        PriorityQueue<KeyValuePair<AIState, List<Action>>> openStates = new PriorityQueue<KeyValuePair<AIState, List<Action>>>();
        openStates.Enqueue(new KeyValuePair<AIState, List<Action>>(startState, new List<Action>()), 0);

        while(openStates.Count > 0)
        {
            KeyValuePair<AIState, List<Action>> pair = openStates.Dequeue().Value;
            AIState currentState = pair.Key;
            List<Action> currentActions = pair.Value;

            closedStates.Add(currentState, currentActions);

            if (goal.IsGoal(currentState))
                return currentActions;

            List<Action> validActions = GetValidActions(currentState);

            foreach(Action action in validActions)
            {
                AIState newState = action.ApplyAction(currentState);

                if (closedStates.ContainsKey(newState))
                    continue;

                List<Action> latestActions = new List<Action>();
                latestActions.AddRange(currentActions);
                latestActions.Add(action);

                openStates.Enqueue(new KeyValuePair<AIState, List<Action>>(newState, latestActions), currentActions.Count + Heuristic(newState));
            }
        }

        Debug.Log("implement me");
        return null;
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
                if(!ingredient.IsCooking)
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
                dropoffAction = new DropOffAction(state.CurrentTableState.ID);
                validActions.Add(dropoffAction);

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

                //Moving ingredients to a pot
                foreach (int potID in state.PotStateIndexList)
                {
                    PotState pot = state.ItemStateList[potID] as PotState;
                    if (pot.HasCapacity(1))
                    {
                        dropoffAction = new DropOffAction(pot.ID);
                        validActions.Add(dropoffAction);
                    }
                }

                //Moving ingredients to a plate
                foreach (int plateID in state.PlateStateIndexList)
                {
                    PlateState plate = state.ItemStateList[plateID] as PlateState;
                    if (plate.IsFree())
                    {
                        dropoffAction = new DropOffAction(plate.ID);
                        validActions.Add(dropoffAction);
                    }
                }
            }

            if(type == ItemType.POT)
            {
                //Putting things on the table
                dropoffAction = new DropOffAction(state.CurrentTableState.ID);
                validActions.Add(dropoffAction);

                
                if(true)
                {
                    throw new System.NotImplementedException("Need a way to check that the pot is non-empty");

                    //Moving the meal to a pot
                    foreach (int potID in state.PotStateIndexList)
                    {
                        PotState pot = state.ItemStateList[potID] as PotState;
                        if (pot.ID == state.CurrentPlayerState.HoldingItemID)
                            continue;

                        throw new System.NotImplementedException("Need to check that the pot has the capacity to store the plate's contents");
                        if (pot.HasCapacity(200))
                        {
                            transferAction = new TransferAction(pot.ID);
                            validActions.Add(transferAction);
                        }
                    }

                    //Moving the meal to a plate
                    foreach (int plateID in state.PlateStateIndexList)
                    {
                        PlateState plate = state.ItemStateList[plateID] as PlateState;
                        if (plate.IsFree())
                        {
                            transferAction = new TransferAction(plate.ID);
                            validActions.Add(transferAction);
                        }
                    }
                }
            }

            if (type == ItemType.PLATE)
            {
                //Putting things on the table
                dropoffAction = new DropOffAction(state.CurrentTableState.ID);
                validActions.Add(dropoffAction);

                //If the plate is non-empty
                if (true)
                {
                    throw new System.NotImplementedException("Need a way to check that the plate is non-empty");

                    //Submitting the meal
                    SubmitOrderAction submitAction = new SubmitOrderAction();
                    validActions.Add(submitAction);

                    //Moving to a pot
                    foreach (int potID in state.PotStateIndexList)
                    {
                        PotState pot = state.ItemStateList[potID] as PotState;
                        throw new System.NotImplementedException("Need to check that the pot has the capacity to store the plate's contents");
                        if (pot.HasCapacity(200))
                        {
                            transferAction = new TransferAction(pot.ID);
                            validActions.Add(transferAction);
                        }
                    }

                    //Moving the meal to a plate
                    foreach (int plateID in state.PlateStateIndexList)
                    {
                        PlateState plate = state.ItemStateList[plateID] as PlateState;
                        if (plate.ID == state.CurrentPlayerState.HoldingItemID)
                            continue;

                        if (plate.IsFree())
                        {
                            transferAction = new TransferAction(plate.ID);
                            validActions.Add(transferAction);
                        }
                    }
                }
            }
        }


        Debug.Log("Implement me!!");
        return null;
    }

    /// <summary>
    /// Returns a heuristic for the distance of the current game state to the goal
    /// </summary>
    /// <returns></returns>
    public float Heuristic(AIState state)
    {
        Debug.Log("Implement a heuristic here");
        return 0;
    }
}

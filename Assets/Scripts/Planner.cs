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
            foreach (IngredientState ingredient in state.IngredientStateList)
            {
                if(!ingredient.IsCooking)
                {
                    pickupAction = new PickUpAction(ingredient.ID);
                    validActions.Add(pickupAction);
                }
            }

            //Pots
            foreach(PotState pot in state.PotStateList)
            {
                pickupAction = new PickUpAction(pot.ID);
                validActions.Add(pickupAction);
            }

            //Plates
            foreach (PlateState plate in state.PlateStateList)
            {
                pickupAction = new PickUpAction(plate.ID);
                validActions.Add(pickupAction);
            }

        }

        //Things you can do when you have something in hand
        else
        {
            DropOffAction dropoffAction;
            ItemType type = itemManager.GetItem(state.CurrentPlayerState.HoldingItemID).MyItemType;

            if (type == ItemType.INGREDIENT)
            {
                //Putting things on the table
                dropoffAction = new DropOffAction(state.CurrentTableState.ID);
                validActions.Add(dropoffAction);

                //Moving ingredients to a cutting board
                foreach (BoardState board in state.BoardStateList)
                {
                    if(board.IsFree())
                    {
                        dropoffAction = new DropOffAction(board.ID);
                        validActions.Add(dropoffAction);
                    }
                }


                //Moving ingredients to a pot
                foreach (PotState pot in state.PotStateList)
                {
                    if (pot.IsFree())
                    {
                        dropoffAction = new DropOffAction(pot.ID);
                        validActions.Add(dropoffAction);
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

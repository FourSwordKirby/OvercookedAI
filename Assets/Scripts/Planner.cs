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

            List<Action> validActions = getValidActions(currentState);

            foreach(Action action in validActions)
            {
                AIState newState = action.ApplyAction(currentState);

                if (closedStates.ContainsKey(newState))
                    continue;

                List<Action> latestActions = new List<Action>();
                latestActions.AddRange(currentActions);
                latestActions.Add(action);

                openStates.Enqueue(new KeyValuePair<AIState, List<Action>>(newState, latestActions), currentActions.Count + heuristic(newState));
            }
        }

        Debug.Log("implement me");
        return null;
    }

    /// <summary>
    /// Used to get all of the valid actions for the current state
    /// </summary>
    /// <returns></returns>
    public List<Action> getValidActions(AIState state)
    {
        Debug.Log("Implement me!!");
        return null;
    }

    /// <summary>
    /// Returns a heuristic for the distance of the current game state to the goal
    /// </summary>
    /// <returns></returns>
    public float heuristic(AIState state)
    {
        Debug.Log("Implement a heuristic here");
        return 0;
    }
}

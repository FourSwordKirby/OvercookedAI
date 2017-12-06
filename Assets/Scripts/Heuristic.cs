using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Heuristic {
    float GetHeuristic(AIState state);
}

public class IngredientBasedHeuristic : Heuristic
{
    private FinishedMealGoal Goal;

    public IngredientBasedHeuristic(FinishedMealGoal goal)
    {
        Goal = goal;
    }

    public float GetHeuristic(AIState state)
    {
        throw new NotImplementedException();
    }
}

public class DumbHeuristic : Heuristic
{
    public float GetHeuristic(AIState state)
    {
        int h = 0;
        float epsilon = 2.0f;


        foreach (int ingredientID in state.IngredientStateIndexList)
        {
            IngredientState ingredient = (state.ItemStateList[ingredientID] as IngredientState);
            if (ingredient.ingredientType == IngredientType.ONION)
            {
                if (!ingredient.IsSpawned)
                    h += 1;
                if (!ingredient.IsPrepared)
                    h += 1;
                if (!ingredient.IsInMeal)
                    h += 1;
            }
        }

        //Debug.Log("Implement a heuristic here");
        return h * epsilon;
    }
}



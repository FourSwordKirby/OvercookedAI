using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Goal
{
    bool IsGoal(AIState currentState);
}

/// <summary>
/// This defines the conditions for completing a single recipe
/// </summary>
public class CookGoal : Goal
{
    public bool IsGoal(AIState currentState)
    {
        foreach(int ingredientID in currentState.IngredientStateIndexList)
        {
            IngredientState ingredient = currentState.ItemStateList[ingredientID] as IngredientState;

            if(ingredient.ingredientType == IngredientType.ONION)
            {
                if (ingredient.IsPrepared == false)
                    return false;
            }
        }
        return true;
    }
}

/// <summary>
/// This defines the conditions for completing a single recipe
/// </summary>
public class RecipeGoal : Goal
{
    public bool IsGoal(AIState currentState)
    {
        Debug.Log("NEEDS TO BE IMPLEMENTED");
        return true;
    }
}

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

public class FinishedMealGoal : Goal
{
    public bool IsGoal(AIState currentState)
    {
        //foreach (int mealID in currentState.MealStateIndexList)
        //{
        //    MealState meal = currentState.ItemStateList[mealID] as MealState;
        //    if (meal.IsCooked())
        //    {
        //        foreach (int ingredientID in meal.ContainedIngredientIDs)
        //        {
        //            if ((currentState.ItemStateList[ingredientID] as IngredientState).ingredientType == IngredientType.ONION)
        //                onionCount--;
        //            if ((currentState.ItemStateList[ingredientID] as IngredientState).ingredientType == IngredientType.MUSHROOM)
        //                mushroomCount--;
        //        }
        //    }
        //}

        foreach(int plateID in currentState.PlateStateIndexList)
        {
            PlateState plate = currentState.ItemStateList[plateID] as PlateState;
            if (plate.IsSubmitted)
            {
                int onionCount = 2;
                int mushroomCount = 1;

                MealState meal = currentState.ItemStateList[plate.mealID] as MealState;

                foreach (int ingredientID in meal.ContainedIngredientIDs)
                {
                    if ((currentState.ItemStateList[ingredientID] as IngredientState).ingredientType == IngredientType.ONION)
                        onionCount--;
                    if ((currentState.ItemStateList[ingredientID] as IngredientState).ingredientType == IngredientType.MUSHROOM)
                        mushroomCount--;
                }
                return onionCount == 0 && mushroomCount == 0;
            }
        }

        return false;
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

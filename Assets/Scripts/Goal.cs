using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<List<IngredientType>> GoalRecipes;

    private List<List<int>> IngredientCountsPerRecipe;
    private const int NUM_INGREDIENT_TYPES = 2;

    public FinishedMealGoal(List<List<IngredientType>> recipes)
    {
        GoalRecipes = recipes.Select(r => new List<IngredientType>(r)).ToList();
        IngredientCountsPerRecipe = new List<List<int>>();
        foreach (List<IngredientType> recipe in recipes)
        {
            List<int> counts = new List<int>(NUM_INGREDIENT_TYPES);
            for (int i = 0; i < NUM_INGREDIENT_TYPES; ++i)
            {
                counts.Add(recipe.Count(ingredientType =>  ingredientType == (IngredientType)i));
            }
            IngredientCountsPerRecipe.Add(counts);
        }
    }

    public bool IsGoal(AIState currentState)
    {
        // This function needs to be fast...
        // Assumes only Mushrooms and Onions
        bool[] plateUsed = new bool[currentState.PlateStateIndexList.Count];
        foreach (List<int> count in IngredientCountsPerRecipe)
        {
            bool found = false;
            for(int plateIndex = 0; plateIndex < currentState.PlateStateIndexList.Count; ++plateIndex)
            {
                int plateID = currentState.PlateStateIndexList[plateIndex];
                PlateState plate = currentState.ItemStateList[plateID] as PlateState;
                if (plate.IsSubmitted && !plateUsed[plateIndex])
                {
                    MealState meal = currentState.ItemStateList[plate.mealID] as MealState;
                    int onionCount = 0;
                    int mushroomCount = 0;
                    foreach (int ingredientID in meal.ContainedIngredientIDs)
                    {
                        IngredientState iState = currentState.ItemStateList[ingredientID] as IngredientState;
                        if (iState.ingredientType == IngredientType.ONION)
                        {
                            ++onionCount;
                        }
                        else
                        {
                            ++mushroomCount;
                        }
                    }

                    if(onionCount == count[(int)IngredientType.ONION]
                       && mushroomCount == count[(int)IngredientType.MUSHROOM])
                    {
                        found = true;
                        plateUsed[plateIndex] = true;
                        break;
                    }
                }
            }

            if(!found)
            {
                return false;
            }
        }
        
        return true;
    }

    public override string ToString()
    {
        return "Goal(" + GoalRecipes.Select(recipe => recipe.ListToString()).ListToString() + ")";
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

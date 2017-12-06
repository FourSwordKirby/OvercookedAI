using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface Heuristic {
    float GetHeuristic(AIState state);
}

public class IngredientBasedHeuristic : Heuristic
{
    private FinishedMealGoal Goal;
    private List<IngredientType> NeededIngredients;
    private List<int> CountOfEachIngredient;

    private List<List<int>> HeuristicPerIngredient;

    public IngredientBasedHeuristic(FinishedMealGoal goal)
    {
        Goal = goal;
        NeededIngredients = Goal.GoalRecipes.SelectMany(r => r).ToList();

        int NUM_INGREDIENT_TYPES = Enum.GetNames(typeof(IngredientType)).Length;
        CountOfEachIngredient = Enumerable.Repeat(0, NUM_INGREDIENT_TYPES).ToList();
        foreach (IngredientType it in NeededIngredients)
        {
            ++CountOfEachIngredient[(int)it];
        }
    }

    public void Init(AIState state)
    {
        // This preallocates an array to hold all of the heuristic numbers.
        int NUM_INGREDIENT_TYPES = Enum.GetNames(typeof(IngredientType)).Length;
        HeuristicPerIngredient = new List<List<int>>(NUM_INGREDIENT_TYPES);
        for (int ingredientType = 0; ingredientType < NUM_INGREDIENT_TYPES; ++ingredientType)
        {
            int count = 0;
            foreach (int ingID in state.IngredientStateIndexList)
            {
                IngredientState iState = state.ItemStateList[ingID] as IngredientState;
                if (ingredientType == (int)iState.ingredientType)
                {
                    ++count;
                }
            }
            HeuristicPerIngredient.Add(new List<int>(count));
        }
    }

    public float GetHeuristic(AIState state)
    {
        if(state.Heuristic >= 0)
        {
            return state.Heuristic;
        }

        if (HeuristicPerIngredient == null)
        {
            Init(state);
        }
        else
        {
            for (int i = 0; i < HeuristicPerIngredient.Count; ++i)
            {
                HeuristicPerIngredient[i].Clear();
            }
        }

        foreach (int ingredientID in state.IngredientStateIndexList)
        {
            IngredientState iState = state.ItemStateList[ingredientID] as IngredientState;
            HeuristicPerIngredient[(int)iState.ingredientType].Add(IngredientHeuristic(iState));
        }

        int ingredientHeuristicSum = 0;
        for (int i = 0; i < HeuristicPerIngredient.Count; ++i)
        {
            HeuristicPerIngredient[i].Sort();
            ingredientHeuristicSum += HeuristicPerIngredient[i].Take(CountOfEachIngredient[i]).Sum();
        }

        int cooktimeHeuristic = 0;
        int submittedHeuristic = 0;
        int[,] mealIngredientCounts = state.GetMealIngredientCounts();
        for (int goalIndex = 0; goalIndex < Goal.GoalRecipes.Count; ++goalIndex)
        {
            int neededCookTime = Goal.GoalRecipes[goalIndex].Count * MealState.COOK_TIME_PER_INGREDIENT + 1;
            int minGoalCookTime = neededCookTime;
            bool submitted = false;
            List<int> goalRecipe = Goal.IngredientCountsPerRecipe[goalIndex];
            for (int mealListIndex = 0; mealListIndex < state.MealStateIndexList.Count; ++mealListIndex)
            {
                MealState meal = state.ItemStateList[state.MealStateIndexList[mealListIndex]] as MealState;
                if (!meal.IsSpawned())
                {
                    continue;
                }

                int onionCount = mealIngredientCounts[mealListIndex, (int)IngredientType.ONION];
                int mushroomCount = mealIngredientCounts[mealListIndex, (int)IngredientType.MUSHROOM];
                if (meal.IsBurnt()
                    || onionCount > goalRecipe[(int)IngredientType.ONION]
                    || mushroomCount > goalRecipe[(int)IngredientType.MUSHROOM])
                {
                    continue;
                }

                // Found qualifying meal.
                bool foundPlate = false;

                // Is there a plate holding this meal...?
                foreach (int plateID in state.PlateStateIndexList)
                {
                    PlateState plate = state.ItemStateList[plateID] as PlateState;
                    if (plate.mealID != meal.ID)
                    {
                        continue;
                    }

                    foundPlate = true;
                    if (plate.IsSubmitted)
                    {
                        // Check if the meal completely matches
                        if (meal.IsCooked()
                            && onionCount == goalRecipe[(int)IngredientType.ONION]
                            && mushroomCount == goalRecipe[(int)IngredientType.MUSHROOM])
                        {
                            // Good job.
                            submitted = true;
                            minGoalCookTime = 0;
                        }
                        else
                        {
                            // Submitted meal that doesn't match the recipe or is uncooked
                            // skip.
                        }
                    }
                    else
                    {
                        // Qualifying meal on a plate that hasn't been submitted.
                        int currentCookDuration = Mathf.Min(meal.cookDuration, meal.ContainedIngredientIDs.Count * MealState.COOK_TIME_PER_INGREDIENT + 1);
                        int remainingCookTime = Mathf.Max(0, neededCookTime - currentCookDuration);
                        minGoalCookTime = Mathf.Min(minGoalCookTime, remainingCookTime);
                    }
                    break;
                }

                if (!foundPlate)
                {
                    // Meal is in a pot.
                    int currentCookDuration = Mathf.Min(meal.cookDuration, meal.ContainedIngredientIDs.Count * MealState.COOK_TIME_PER_INGREDIENT + 1);
                    int remainingCookTime = Mathf.Max(0, neededCookTime - currentCookDuration);
                    minGoalCookTime = Mathf.Min(minGoalCookTime, remainingCookTime);
                }
            }

            //cooktimeHeuristic = Mathf.Max(cooktimeHeuristic, minGoalCookTime);
            cooktimeHeuristic += minGoalCookTime;
            submittedHeuristic += submitted ? 0 : 1;
        }

        //state.Heuristic = Mathf.Max(ingredientHeuristicSum + submittedHeuristic, cooktimeHeuristic);
        //state.Heuristic = ingredientHeuristicSum + submittedHeuristic;
        float parallelism = Mathf.Min(Goal.GoalRecipes.Count, state.PotStateIndexList.Count);
        state.Heuristic = ingredientHeuristicSum + (cooktimeHeuristic / parallelism)  + submittedHeuristic;
        return state.Heuristic;
    }
    
    private int IngredientHeuristic(IngredientState iState)
    {
        if(!iState.IsSpawned)
        {
            return 3;
        }

        if(!iState.IsPrepared)
        {
            return 2;
        }

        if(!iState.IsInMeal)
        {
            return 1;
        }

        return 0;
    }

    public override string ToString()
    {
        return "IngredientBasedHeuristic(" + Goal + ")";
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

    public override string ToString()
    {
        return "DumbHeuristic";
    }
}



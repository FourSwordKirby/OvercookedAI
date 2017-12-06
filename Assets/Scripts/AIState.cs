using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIState : ICloneable
{
    public List<ItemState> ItemStateList;

    // TODO: Technically, these lists don't change. We can probably move them to somewhere outside the state.
    public List<int> IngredientStateIndexList;
    public List<int> PotStateIndexList;
    public List<int> PlateStateIndexList;
    public List<int> MealStateIndexList;
    public List<int> BoardStateIndexList;
    public List<int> TableStateIndexList;
    public List<int> PlateToMeal;

    public PlayerState CurrentPlayerState;

    public const int NUM_INGREDIENT_TYPES = 2;
    public const int MAX_INGREDIENT_TYPE_SPAWN = 5;
    public const int MAX_MEAL_SPAWN = 5;

    public int onionSpawnCount;
    public int mushroomSpawnCount;

    public int GValue;
    public bool IsClosed;
    public AIState Parent;
    public Action ParentAction;

    // Cached values. Do not copy on clone.
    public float Heuristic;
    public int[,] MealIngredientCounts;

    public AIState()
    {
        GValue = int.MaxValue;
        IsClosed = false;
        Parent = null;
        ParentAction = null;
        Heuristic = -1f;
    }

    public object Clone()
    {
        return new AIState()
        {
            ItemStateList = this.ItemStateList.Select(itemState => itemState.Clone() as ItemState).ToList(),
            IngredientStateIndexList = this.IngredientStateIndexList,
            PotStateIndexList = this.PotStateIndexList,
            PlateStateIndexList = this.PlateStateIndexList,
            MealStateIndexList = this.MealStateIndexList,
            BoardStateIndexList = this.BoardStateIndexList,
            TableStateIndexList = this.TableStateIndexList,
            CurrentPlayerState = this.CurrentPlayerState.Clone() as PlayerState,
            onionSpawnCount = this.onionSpawnCount,
            mushroomSpawnCount = this.mushroomSpawnCount,
            PlateToMeal = this.PlateToMeal
        };
    }

    public int[,] GetMealIngredientCounts()
    {
        if (MealIngredientCounts != null)
        {
            return MealIngredientCounts;
        }

        MealIngredientCounts = new int[MealStateIndexList.Count, NUM_INGREDIENT_TYPES];
        for (int i = 0; i < MealStateIndexList.Count; ++i)
        {
            MealState meal = ItemStateList[MealStateIndexList[i]] as MealState;
            foreach (int ingID in meal.ContainedIngredientIDs)
            {
                IngredientState ingredient = ItemStateList[ingID] as IngredientState;
                ++MealIngredientCounts[i, (int)ingredient.ingredientType];
            }
        }
        return MealIngredientCounts;
    }
}

public class AIStateComparator : IEqualityComparer<AIState>
{
    public bool Equals(AIState x, AIState y)
    {
        return x.ItemStateList.SequenceEqual(y.ItemStateList)
            && x.CurrentPlayerState.Equals(y.CurrentPlayerState);
    }

    public int GetHashCode(AIState obj)
    {
        int seed = 487;
        int modifier = 31;

        unchecked
        {
            int hash = obj.ItemStateList.Aggregate(seed, (current, item) => (current * modifier) + item.GetHashCode());
            hash = (hash * modifier) + obj.CurrentPlayerState.GetHashCode();
            hash = (hash * modifier) + obj.onionSpawnCount;
            hash = (hash * modifier) + obj.mushroomSpawnCount;
            return hash;
        }
    }
}
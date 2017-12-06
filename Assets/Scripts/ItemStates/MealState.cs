using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MealState : ItemState {
    
    public List<int> ContainedIngredientIDs;
    public int cookDuration = 0;

    public const int COOK_TIME_PER_INGREDIENT = 3;

    public MealState(int id, int time, List<int> containingItemIDs)
        : base(id, ItemType.MEAL)
    {
        cookDuration = time;
        ContainedIngredientIDs = containingItemIDs;
    }

    public bool IsSpawned()
    {
        return ContainedIngredientIDs.Count > 0;
    }

    public int MealSize()
    {
        return ContainedIngredientIDs.Count;
    }

    public void CookIngredients()
    {
        cookDuration++;
    }

    public bool IsCooked()
    {
        return cookDuration > ContainedIngredientIDs.Count * COOK_TIME_PER_INGREDIENT;
    }

    public bool IsBurnt()
    {
        return cookDuration > (ContainedIngredientIDs.Count + 2) * COOK_TIME_PER_INGREDIENT;
    }

    public override object Clone()
    {
        MealState newState = MemberwiseClone() as MealState;
        newState.ContainedIngredientIDs = new List<int>(newState.ContainedIngredientIDs);
        return newState;
    }

    public float CurrentCookingProgress()
    {
        return Mathf.Max(0, (float)(cookDuration) / (float)(ContainedIngredientIDs.Count * COOK_TIME_PER_INGREDIENT));
    }

    public float CurrentBurningProgress()
    {
        return Mathf.Max(0, (float)(cookDuration - ContainedIngredientIDs.Count * COOK_TIME_PER_INGREDIENT) / (float)(2 * COOK_TIME_PER_INGREDIENT));
    }


    public override bool Equals(object obj)
    {
        MealState otherState = obj as MealState;
        if (otherState == null)
        {
            return false;
        }

        if (otherState == this)
        {
            return true;
        }

        return this.ID == otherState.ID
            && this.cookDuration == otherState.cookDuration
            && this.ContainedIngredientIDs.All(id => otherState.ContainedIngredientIDs.Contains(id));

    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            ret = 33 * ret + ID;
            ret = 33 * ret + cookDuration;

            int grouphash = 0;
            foreach (int id in ContainedIngredientIDs)
            {
                grouphash ^= Utility.HashInt(id);
            }
            ret = 33 * ret + grouphash;

            return ret;
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using System;

public class MealState : ItemState {

    public bool IsSpawned;
    public List<int> ContainedIngredientIDs;

    public int cookDuration = 0;

    public MealState(int id, bool isSpawned, List<int> containingItemIDs)
        : base(id, ItemType.MEAL)
    {
        IsSpawned = isSpawned;
        ContainedIngredientIDs = containingItemIDs;
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
        return cookDuration > ContainedIngredientIDs.Count * 3;
    }

    public bool IsBurnt()
    {
        return cookDuration > (ContainedIngredientIDs.Count + 2) * 3;
    }

    public override object Clone()
    {
        return new MealState(ID, IsSpawned, new List<int>(ContainedIngredientIDs));
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
            && this.ContainedIngredientIDs.All(id => otherState.ContainedIngredientIDs.Contains(id));

    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            ret = 33 * ret + ID;
            foreach (int id in ContainedIngredientIDs)
            {
                ret = 33 * ret + id;
            }
            return ret;
        }
    }
}

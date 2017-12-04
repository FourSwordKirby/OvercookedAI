using System;
using System.Collections.Generic;
using System.Linq;

public class PotState : ItemState
{
    public const int MAX_ITEMS_PER_POT = 3;
    public int currentMealSize;

    public int mealID;

    public PotState(int id, int MealID)
        : base (id, ItemType.POT)
    {
        this.mealID = MealID;
    }

    public bool IsEmpty()
    {
        return currentMealSize == 0;
    }

    public bool HasCapacity(int AddedItemCount)
    {
        return MAX_ITEMS_PER_POT > currentMealSize;
    }

    public override object Clone()
    {
        return new PotState(ID, mealID);
    }

    public override bool Equals(object obj)
    {
        PotState otherState = obj as PotState;
        if (otherState == null)
        {
            return false;
        }

        if (otherState == this)
        {
            return true;
        }

        return this.ID == otherState.ID
            && this.mealID == otherState.mealID;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            ret = 33 * ret + ID;
            return ret;
        }
    }
}

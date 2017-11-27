using System.Linq;
using System.Collections.Generic;
using System;

public class SoupState : ItemState {

    const int MAX_INGREDIENTS_PER_SOUP = 3;

    public bool IsSpawned;
    public List<int> ContainedIngredientIDs;

    private SoupState(int id, List<int> containedIngredientIDs)
        : base(id)
    {
        ContainedIngredientIDs = containedIngredientIDs;
    }

    public SoupState(int id)
        : base(id)
    {
        IsSpawned = false;
        ContainedIngredientIDs = Enumerable.Repeat(Item.NOTHING_ID, MAX_INGREDIENTS_PER_SOUP).ToList();
    }

    public override object Clone()
    {
        return new SoupState(ID, new List<int>(ContainedIngredientIDs));
    }

    public override bool Equals(object obj)
    {
        SoupState otherState = obj as SoupState;
        if (otherState == null)
        {
            return false;
        }

        if (this == otherState)
        {
            return true;
        }

        return this.ID == otherState.ID
            && this.IsSpawned == otherState.IsSpawned
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
            ret = (ret << 1) + (IsSpawned ? 1 : 0);
            return ret;
        }
    }
}

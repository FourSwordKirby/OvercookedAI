using System;
using System.Collections.Generic;
using System.Linq;

public class PotState : ItemState
{
    public const int MAX_ITEMS_PER_POT = 3;

    public List<int> ItemIDsInPot;

    private PotState(int id, List<int> containingItemIDs)
        : base (id)
    {
        ItemIDsInPot = containingItemIDs;
    }

    public PotState(int id) : base(id)
    {
        ItemIDsInPot = Enumerable.Repeat(Item.NOTHING_ID, MAX_ITEMS_PER_POT).ToList();
    }

    public bool HasCapacity(int AddedItemCount)
    {
        throw new NotImplementedException();
    }

    public override object Clone()
    {
        return new PotState(ID, new List<int>(ItemIDsInPot));
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
            && this.ItemIDsInPot.All(id => otherState.ItemIDsInPot.Contains(id));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            ret = 33 * ret + ID;
            foreach (int id in ItemIDsInPot)
            {
                ret = 33 * ret + id;
            }
            return ret;
        }
    }
}

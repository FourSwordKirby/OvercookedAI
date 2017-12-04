using System;

public class PlateState : ItemState
{
    public int MealID;

    public PlateState(int id) : base(id, ItemType.PLATE)
    {
        MealID = Item.NOTHING_ID;
    }

    public bool IsEmpty()
    {
        return MealID == Item.NOTHING_ID;
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (this == obj)
        {
            return true;
        }

        PlateState otherState = obj as PlateState;
        if (obj == null)
        {
            return false;
        }

        return this.ID == otherState.ID
            && this.MealID == otherState.MealID;
    }

    public override int GetHashCode()
    {
        return (ID << 16) | MealID;
    }
}

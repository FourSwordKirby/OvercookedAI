using System;

public class PlateState : ItemState
{
    public int mealID;
    public bool IsSubmitted;

    public PlateState(int id, int mealID, bool isSubmitted) : base(id, ItemType.PLATE)
    {
        this.mealID = mealID;
        IsSubmitted = isSubmitted;
    }

    public bool IsEmpty()
    {
        return mealID == Item.NOTHING_ID;
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
            && this.mealID == otherState.mealID
            && this.IsSubmitted == otherState.IsSubmitted;
    }

    public override int GetHashCode()
    {
        return (ID << 16) | (mealID << 1) | (IsSubmitted ? 1 : 0);
    }
}
